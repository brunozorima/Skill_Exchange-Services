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
        public Task<ExchangeRequest> AddExchangeRequest(ExchangeRequest exchangeRequest, CancellationToken cancellationToken);
        public Task<ExchangeMessage> AddExchangeMessage(ExchangeMessage exchangeMessage, CancellationToken cancellationToken);

    }
}
