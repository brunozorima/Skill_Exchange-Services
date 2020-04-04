using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using SkillExchange.AccessService.Domain;
using SkillExchange.AccessService.Domain.ExchangeDomain;
using SkillExchange.AccessService.Models;
using SkillExchange.AccessService.Repository.Exchange_Repository;

namespace SkillExchange.AccessService.Services.ExchangeService
{
    public class ExchangeService : IExchangeService
    {
        public readonly IExchangeRepository _exchangeRepository;
        public readonly IIdentityService _identityService;
        public ExchangeService(IExchangeRepository exchangeRepository, IIdentityService identityService)
        {
            this._exchangeRepository = exchangeRepository;
            this._identityService = identityService;
        }
        public async Task<ExchangeResult> SendExchangeMessageAsync(ExchangeMessage exchangeMessage, CancellationToken cancellationToken)
        {
            var messageToExchange = await this._exchangeRepository.SendExchangeMessage(exchangeMessage, cancellationToken);
            if (messageToExchange > 0)
            {
                return new ExchangeResult
                {
                    Success = true,
                    Returned_Id = messageToExchange
                };

            }
            return new ExchangeResult
            {
                Errors = new[] { "Exchange Message For Skill cannot be SENT!" }
            };
        }

        public async Task<ExchangeResult> RequestExchangeAsync(ExchangeRequest exchangeRequest, CancellationToken cancellationToken)
        {
            var requestForExchange = await this._exchangeRepository.RequestExchange(exchangeRequest, cancellationToken);
            if (requestForExchange > 0)
            {
                return new ExchangeResult
                {
                    Success = true,
                    Returned_Id = requestForExchange
                };

            }
            return new ExchangeResult
            {
                Errors = new[] { "Exchange Request For Skill cannot be REQUESTED!" }
            };
        }
        public async Task<ExchangeRequest> GetExchangeRequestByIdAsync(int request_id, CancellationToken cancellationToken)
        {
            var exchange = await this._exchangeRepository.GetExchangeRequestById(request_id, cancellationToken);
            return exchange;
        }
        //get all messaages for the logged in user
        public async Task<ExchangeResult> GetAllMessagesInOneExchangeAsync(int loggedInUser, int exchange_id, CancellationToken cancellationToken)
        {
            //create an exchange model object to hold the messages
            List<ExchangeResultModel> MessageList = new List<ExchangeResultModel>();

            //get all the messages beloging to an exchange
            var messages = await this._exchangeRepository.GetAllMessagesInOneExchange(loggedInUser, exchange_id, cancellationToken);
            var exchange = await this.GetExchangeRequestByIdAsync(exchange_id, cancellationToken);
            if (messages != null)
            {
                foreach (var message in messages)
                {
                    if (message.Sender_Id == exchange.Recipient_Id) { loggedInUser = exchange.Sender_Id; } else { loggedInUser = exchange.Recipient_Id; }
                    var messageResult = new ExchangeResultModel
                    {
                        From = this._identityService.GetUserByIdAsync(message.Sender_Id).Result.authSuccessResponse.FirstName,
                        To = this._identityService.GetUserByIdAsync(loggedInUser).Result.authSuccessResponse.FirstName,
                        Exchange_Id = message.Exchange_Id,
                        Body = message.Body,
                        TimeStamp = message.TimeStamp
                    };
                    MessageList.Add(messageResult);
                }
                return new ExchangeResult
                {
                    Success = true,
                    ExchangeResultResponse = MessageList.OrderBy(message => message.TimeStamp)
                };
            }
            return new ExchangeResult
            {
                Errors = new[] { "No Messages Found In This Exchange!" }
            };
        }
        public async Task<ExchangeResult> GetMessageById(int message_id, int loggedInUser, CancellationToken cancellationToken)
        {
            var message = await this._exchangeRepository.GetMessageById(message_id, cancellationToken);
            var exchange = await this.GetExchangeRequestByIdAsync(message.Exchange_Id, cancellationToken);
            if (message.Sender_Id == exchange.Recipient_Id) { loggedInUser = exchange.Sender_Id; } else { loggedInUser = exchange.Recipient_Id; }

            if (message != null)
            {
                var messageResult = new ExchangeResultModel
                {
                    From = this._identityService.GetUserByIdAsync(message.Sender_Id).Result.authSuccessResponse.FirstName,
                    To = this._identityService.GetUserByIdAsync(loggedInUser).Result.authSuccessResponse.FirstName,
                    Exchange_Id = message.Exchange_Id,
                    Body = message.Body,
                    TimeStamp = message.TimeStamp
                };
                return new ExchangeResult
                {
                    Success = true,
                    ExchangeMessage = messageResult
                };
            }
            return new ExchangeResult
            {
                Errors = new[] { "No Messages Found In This Exchange!" }
            };
        }
        //get users who I sent requests to
        public async Task<ExchangeResult> RequestSentToAsync(int sender_id, CancellationToken cancellationToken, int status)
        {
            //create a User and Exchange model object for the response
            List<ExchangeResponseModel> exchangeResponseModels = new List<ExchangeResponseModel>();
            //grab all users who request have been sent to...
            var listOfExchange = await this._exchangeRepository.RequestSentTo(sender_id, cancellationToken, status);
            if(listOfExchange != null)
            {
                foreach(var userObj in listOfExchange)
                {
                    var user = new ExchangeResponseModel
                    {
                        User_Id = userObj.User_Id,
                        FirstName = userObj.FirstName,
                        LastName = userObj.LastName,
                        Email = userObj.Email,
                        Exchange_Id = userObj.Exchange_Id,
                        Status = userObj.Status,
                        Opened_TimeStamp = userObj.Opened_TimeStamp
                    };
                    exchangeResponseModels.Add(user);
                }
                return new ExchangeResult
                {
                    Success = true,
                    ExchangeObjectUserModel = exchangeResponseModels
                };
            }
            return new ExchangeResult
            {
                Errors = new[] { "No Sent Requests Found!" }
            };
        }
        //get users who requests I recieved From
        public async Task<ExchangeResult> RequestRecievedFromAsync(int recipient_id, CancellationToken cancellationToken, int status)
        {
            //create a User and Exchange model object for the response
            List<ExchangeResponseModel> exchangeResponseModels = new List<ExchangeResponseModel>();
            //grab all users who sent Requests to this user...
            var listOfExchange = await this._exchangeRepository.RequestRecievedFrom(recipient_id, cancellationToken, status);
            if (listOfExchange != null)
            {
                foreach (var userObj in listOfExchange)
                {
                    var user = new ExchangeResponseModel
                    {
                        User_Id = userObj.User_Id,
                        FirstName = userObj.FirstName,
                        LastName = userObj.LastName,
                        Email = userObj.Email,
                        Exchange_Id = userObj.Exchange_Id,
                        Status = userObj.Status,
                        Opened_TimeStamp = userObj.Opened_TimeStamp
                    };
                    exchangeResponseModels.Add(user);
                }
                return new ExchangeResult
                {
                    Success = true,
                    ExchangeObjectUserModel = exchangeResponseModels
                };
            }
            return new ExchangeResult
            {
                Errors = new[] { "No Recieved Requests Found!" }
            };
        }
        public async Task<ExchangeRequest> UpdateRequestStatusAsync(int request_id, int status, int recipient, CancellationToken cancellationToken)
        {
            var exchange = await this._exchangeRepository.UpdateRequestStatus(request_id, status, recipient, cancellationToken);
            return exchange;
        }
        public async Task<int> RejectRequest(int request_id, int user, CancellationToken cancellationToken)
        {
            var result = await this._exchangeRepository.RejectRequest(request_id, user, cancellationToken);
            return result;
        }
    }
}
