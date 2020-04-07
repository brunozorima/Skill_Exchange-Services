using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SkillExchange.AccessService.Domain.SkillDomain;
using SkillExchange.AccessService.Models;
using SkillExchange.AccessService.Repository;

namespace SkillExchange.AccessService.Services.SkillService
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;
        public readonly IIdentityService _identityService;
        public readonly IPerson_Has_Need_Skill_Service _person_Has_Need_Skill_Service;

        public SkillService(ISkillRepository skillRepository, IIdentityService identityService, IPerson_Has_Need_Skill_Service person_Has_Need_Skill_Service)
        {
            this._skillRepository = skillRepository;
            this._identityService = identityService;
            this._person_Has_Need_Skill_Service = person_Has_Need_Skill_Service;
        }

        public async Task<SkillResult> AddSkillAsync(SkillModel skill, CancellationToken cancellationToken)
        {
            var skillToAdd = await this._skillRepository.AddSkill(skill, cancellationToken);
            if(skillToAdd != null)
            {
                return new SkillResult
                {
                    Success = true,
                    SkillSuccessResponse = skillToAdd
                };
            }
            return new SkillResult
            {
                Errors = new[] { "Skill cannot be added!"}
            };
        }

        public async Task<SkillResult> DeleteSkillAsync(int id, CancellationToken cancellationToken)
        {
            var skillToDelete = await this._skillRepository.GetSkillById(id, cancellationToken);
            if(skillToDelete != null)
            {
                await this._skillRepository.DeleteSkill(id, cancellationToken);
                return new SkillResult
                {
                    Success = true
                };
            }
            return new SkillResult
            {
                Errors = new[] { "Skill does not exist, thus cannot be deleted!" }
            };
        }

        public async Task<IEnumerable<SkillModel>> GetAllSkillsAsync(CancellationToken cancellationToken)
        {
            var availableSkills = await this._skillRepository.GetAllSkillsAsync(cancellationToken);
            return availableSkills;
        }

        public async Task<IEnumerable<SkillModel>> GetSkillByCategoryAsync(int category_id, CancellationToken cancellationToken)
        {
            var result = await this._skillRepository.GetSkillByCategory(category_id, cancellationToken);
            return result;
        }

        public async Task<SkillResult> GetSkillByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await this._skillRepository.GetSkillById(id, cancellationToken);
            if (result == null)
            {
                return new SkillResult
                {
                    Errors = new[] { "This skill attritube does not exist!" }
                };
            }

            var skillobject = new SkillModel
            {
                Id = result.Id,
                Name = result.Name,
                Category = result.Category
            };

            return new SkillResult
            {
                Success = true,
                SkillSuccessResponse = skillobject
            };
        }

        public async Task<IEnumerable<SkillModel>> FindSkillsByNameAsync(string name, CancellationToken cancellationToken)
        {
            var result = await this._skillRepository.FindSkillByName(name, cancellationToken);
            return result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetWantedPersonBySkillIdAsync(int skill_id, CancellationToken cancellationToken)
        {
            var result = await this._skillRepository.GetWantedPersonBySkillId(skill_id, cancellationToken);
            return result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetPersonOwningSkillsSkillIdAsync(int skill_id, CancellationToken cancellationToken)
        {
            var result = await this._skillRepository.GetPersonOwningSkillsBySkillId(skill_id, cancellationToken);
            return result;
        }

        public async Task<UserProfileModel> GetUserSkillDataAsync(int user_id, CancellationToken cancellationToken)
        {            
            var user = await this._identityService.GetUserById(user_id, cancellationToken);           
            var userHaveSkills = await this._person_Has_Need_Skill_Service.GetPersonHasSkillById(user_id, cancellationToken);
            var userNeedSkills = await this._person_Has_Need_Skill_Service.GetPersonNeedSkillById(user_id, cancellationToken);
            if(user != null)
            {
                var userProfileModel = new UserProfileModel
                {
                    UserDetails = user,
                    UserHaveSkills = userHaveSkills,
                    UserNeedSkills = userNeedSkills
                };
                return userProfileModel;
            }
            return null;
        }
    }
}
