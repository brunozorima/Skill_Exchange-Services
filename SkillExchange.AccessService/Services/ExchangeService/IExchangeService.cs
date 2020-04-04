using SkillExchange.AccessService.Domain;
using SkillExchange.AccessService.Domain.ExchangeDomain;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Services.ExchangeService
{
    public interface IExchangeService
    {
        public Task<ExchangeResult> RequestExchangeAsync(ExchangeRequest exchangeRequest, CancellationToken cancellationToken);
        public Task<ExchangeResult> SendExchangeMessageAsync(ExchangeMessage exchangeMessage, CancellationToken cancellationToken);
        public Task<ExchangeResult> GetAllMessagesInOneExchangeAsync(int loggedInUser, int exchange_id, CancellationToken cancellationToken);
        public Task<ExchangeResult> RequestSentToAsync(int sender_id, CancellationToken cancellationToken, int status);
        public Task<ExchangeResult> RequestRecievedFromAsync(int recipient_id, CancellationToken cancellationToken, int status);
        public Task<ExchangeRequest> GetExchangeRequestByIdAsync(int request_id, CancellationToken cancellationToken);
        public Task<ExchangeResult> GetMessageById(int message_id, int loggedInUser, CancellationToken cancellationToken);
        public Task<ExchangeRequest> UpdateRequestStatusAsync(int request_id, int status, int recipient, CancellationToken cancellationToken);
        public Task<int> RejectRequest(int request_id, int user, CancellationToken cancellationToken);
    }
}
