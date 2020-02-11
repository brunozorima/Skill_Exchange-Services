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
        public SkillService(ISkillRepository skillRepository)
        {
            this._skillRepository = skillRepository;
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

        public async Task<IEnumerable<SkillModel>> FindSkillsByName(string name, CancellationToken cancellationToken)
        {
            var result = await this._skillRepository.FindSkillByName(name, cancellationToken);
            return result;
        }
    }
}
