using Microsoft.AspNet.Identity;
using SkillExchange.AccessService.Domain.SkillDomain;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Services.SkillService
{
    public interface ISkillService
    {
        public Task<SkillResult> AddSkillAsync(SkillModel skill, CancellationToken cancellationToken);
        public Task<IEnumerable<SkillModel>> GetAllSkillsAsync(CancellationToken cancellationToken);
        public Task<SkillResult> GetSkillByIdAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<SkillModel>> GetSkillByCategoryAsync(int category_id, CancellationToken cancellationToken);
        public Task<SkillResult> DeleteSkillAsync(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<SkillModel>> FindSkillsByNameAsync(string name, CancellationToken cancellationToken);
        public Task<IEnumerable<ApplicationUser>> GetWantedPersonBySkillIdAsync(int skill_id, CancellationToken cancellationToken);
        public Task<IEnumerable<ApplicationUser>> GetPersonOwningSkillsSkillIdAsync(int skill_id, CancellationToken cancellationToken);
    }
}
