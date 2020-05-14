using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Domain.SkillDomain
{
    public class SkillResult
    {
        public int skillAddedId { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public SkillModel SkillSuccessResponse { get; set; }
    }
}
