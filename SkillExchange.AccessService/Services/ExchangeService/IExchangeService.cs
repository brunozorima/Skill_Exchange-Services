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
        public Task<AuthenticationResult> GetRequestListAsync(int recipient_id, int status, CancellationToken cancellationToken);
    }
}
