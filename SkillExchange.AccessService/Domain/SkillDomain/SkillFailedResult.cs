using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Domain.SkillDomain
{
    public class SkillFailedResult
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
