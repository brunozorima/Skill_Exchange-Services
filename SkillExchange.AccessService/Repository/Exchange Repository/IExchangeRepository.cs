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
        public Task<IEnumerable<ApplicationUser>> GetRequestList(int recipient_id, int status, CancellationToken cancellationToken);
        public Task<IEnumerable<ExchangeResultModel>> GetExchangeMessage(int recipient_id, CancellationToken cancellationToken);


    }
}
