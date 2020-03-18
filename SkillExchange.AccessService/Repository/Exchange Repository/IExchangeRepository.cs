using SkillExchange.AccessService.Domain.ExchangeDomain;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Repository.Exchange_Repository
{
    public interface IExchangeRepository
    {
        public Task<int> RequestExchange(ExchangeRequest exchangeRequest, CancellationToken cancellationToken);
        public Task<int> SendExchangeMessage(ExchangeMessage exchangeMessage, CancellationToken cancellationToken);
        public Task<IEnumerable<ExchangeMessage>> GetAllMessagesInOneExchange(int loggedInUserId, int exchange_id, CancellationToken cancellationToken);
        public Task<IEnumerable<ExchangeResponseModel>> RequestSentTo(int sender_id, CancellationToken cancellationToken, int status);
        public Task<IEnumerable<ExchangeResponseModel>> RequestRecievedFrom(int recipient_id, CancellationToken cancellationToken, int status);
        public Task<ExchangeRequest> GetExchangeRequestById(int request_id, CancellationToken cancellationToken);
        public Task<ExchangeMessage> GetMessageById(int message_id, CancellationToken cancellationToken);
    }
}
