using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Domain.ExchangeDomain
{
    public class ExchangeFailedResult
    {
        public IEnumerable<string> Errors { get; set; }

    }
}
