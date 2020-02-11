using Microsoft.AspNet.Identity;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Services.SkillService
{
    public interface IPerson_Has_Need_Skill_Service
    {
        public Task<IdentityResult> AddPersonHasSkillById(int Person_Id, int Skill_Id, CancellationToken cancellationToken);
        public Task<IEnumerable<SkillModel>> GetPersonHasSkillById(int Person_Id, CancellationToken cancellationToken);
    }
}
