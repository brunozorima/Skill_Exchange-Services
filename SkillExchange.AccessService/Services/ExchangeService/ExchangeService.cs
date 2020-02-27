using System;
using System.Collections.Generic;
using System.Linq;
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
        public ExchangeService(IExchangeRepository exchangeRepository)
        {
            this._exchangeRepository = exchangeRepository;
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

        public async Task<AuthenticationResult> GetRequestListAsync(int recipient_id, int status, CancellationToken cancellationToken)
        {

            var requestList = await this._exchangeRepository.GetRequestList(recipient_id, status, cancellationToken);
            if (requestList != null)
            {
                return new AuthenticationResult
                {
                    Success = true,
                    requestedUser = requestList
                };

            }
            return new AuthenticationResult
            {
                Errors = new[] { "No Exchange Requests Avaliable!" }
            };
        }


    }
}
