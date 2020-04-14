using Microsoft.AspNet.Identity;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Repository
{
    public interface ISkillRepository
    {
        public Task<IEnumerable<SkillModel>> GetAllSkillsAsync(CancellationToken cancellationToken);
        public Task<SkillModel> GetSkillById(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<SkillModel>> GetSkillByCategory(int category_id, CancellationToken cancellationToken);
        public Task<SkillModel> AddSkill(SkillModel skillModel, CancellationToken cancellationToken);
        public Task<IdentityResult> DeleteSkill(int id, CancellationToken cancellationToken);
        public Task<IEnumerable<SkillModel>> FindSkillByName(string name, CancellationToken cancellationToken);
        public Task<IEnumerable<ApplicationUser>> GetWantedPersonBySkillId(int skill_id, CancellationToken cancellationToken);
        public Task<IEnumerable<ApplicationUser>> GetPersonOwningSkillsBySkillId(int skill_id, CancellationToken cancellationToken);
        public Task<IEnumerable<int>> GetAutoMatch(int loggedInUser, CancellationToken cancellationToken);
        public Task<IEnumerable<int>> GetPeopleWithSkillsWant(int loggedInUser, CancellationToken cancellationToken);
        public Task<IEnumerable<int>> GetPeopleWithSkillsHave(int loggedInUser, CancellationToken cancellationToken);
    }
}
