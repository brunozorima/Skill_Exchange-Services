 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Models
{
    public class UserProfileModel
    {
        public ApplicationUser UserDetails { get; set; }
        public IEnumerable<SkillModel> UserHaveSkills { get; set; }
        public IEnumerable<SkillModel> UserNeedSkills { get; set; }

    }
}
