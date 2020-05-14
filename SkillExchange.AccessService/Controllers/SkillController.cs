using Microsoft.AspNetCore.Mvc;
using SkillExchange.AccessService.Domain.SkillDomain;
using SkillExchange.AccessService.Models;
using SkillExchange.AccessService.Services.SkillService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Controllers
{
    //controller name  = access
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly IPerson_Has_Need_Skill_Service _person_Has_Need_Skill_Service;

        public SkillController(ISkillService skillService, IPerson_Has_Need_Skill_Service person_Has_Need_Skill_Service)
        {
            this._skillService = skillService;
            this._person_Has_Need_Skill_Service = person_Has_Need_Skill_Service;
        }
        //todo: fix the add skill method. it thrown an error
        [HttpPost]
        [Route("/api/[controller]/create/name={name}/cat={category}/type={type}/user={belongsTo}")]
        public async Task<IActionResult> AddSkill(string name, int category, int type, int belongsTo, CancellationToken cancellationToken)
        {
            var constSkill = new SkillModel
            {
                Name = name,
                Category = (Category)category
            };
            var result = await this._skillService.AddSkillAsync(constSkill, type, belongsTo, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(new SkillFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.SkillSuccessResponse);
        }

        [HttpGet]
        [Route("/api/[controller]/skills")]
        public async Task<IEnumerable<SkillModel>> GetSkillModelsAsync(CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetAllSkillsAsync(cancellationToken);
            return result.OrderBy(skill => skill.Name);

        }

        [HttpDelete]
        [Route("/api/[controller]/{id}")]
        public async Task<IActionResult> DeleteSkill(int id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.DeleteSkillAsync(id, cancellationToken);
            if (result.Success)
            {
                return BadRequest(new SkillFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok();
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        public async Task<IActionResult> GetSkillByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetSkillByIdAsync(id, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(new SkillFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.SkillSuccessResponse);
        }

        [HttpGet]
        [Route("/api/[controller]/category/{category_id}")]
        public async Task<IEnumerable<SkillModel>> GetSkillByCategoryAsync(int category_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetSkillByCategoryAsync(category_id, cancellationToken);
            return result;
        }

        [HttpGet]
        [Route("/api/[controller]/findByName={name}")]
        public async Task<IEnumerable<SkillModel>> FindSkillsByName(string name, CancellationToken cancellationToken)
        {
            var result = await this._skillService.FindSkillsByNameAsync(name, cancellationToken);
            return result;
        }

        [HttpGet]
        [Route("/api/[controller]/{skill_id}/wantedPeople")]
        public async Task<IActionResult> GetWantedPersonBySkillIdAsync(int skill_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetWantedPersonBySkillIdAsync(skill_id, cancellationToken);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not Users Found!");
        }
        
        [HttpGet]
        [Route("/api/[controller]/{skill_id}/owningPeople")]
        public async Task<IActionResult> GetPersonOwningSkillsBySkillIdAsync(int skill_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetPersonOwningSkillsSkillIdAsync(skill_id, cancellationToken);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not Users Found!");
        }
    }
}
