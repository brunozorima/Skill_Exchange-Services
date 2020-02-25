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
        [Route("/api/[controller]/skill")]
        public async Task<IActionResult> AddSkill([FromBody] SkillModel skillToAdd, CancellationToken cancellationToken)
        {
            var result = await this._skillService.AddSkillAsync(skillToAdd, cancellationToken);
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
        public async Task<IEnumerable<ApplicationUser>> GetWantedPersonBySkillIdAsync(int skill_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetWantedPersonBySkillIdAsync(skill_id, cancellationToken);
            return result;
        }
        
        [HttpGet]
        [Route("/api/[controller]/{skill_id}/owningPeople")]
        public async Task<IEnumerable<ApplicationUser>> GetPersonOwningSkillsBySkillIdAsync(int skill_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetPersonOwningSkillsSkillIdAsync(skill_id, cancellationToken);
            return result;
        }

        ///// <summary>
        ///// ////////
        ///// </summary>
        ///// <param name="Person_Id"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        ////People has skill api
        //[HttpGet]
        //[Route("/api/person/{person_id}/ownSkills")]
        //public async Task<IEnumerable<SkillModel>> GetPersonHasSkillById(int person_id, CancellationToken cancellationToken)
        //{
        //    var result = await this._person_Has_Need_Skill_Service.GetPersonHasSkillById(person_id, cancellationToken);
        //    return result;
        //}
             
        //[HttpPost]
        //[Route("/api/[controller]/PersonHasSkill/person={Person_Id}/skill={Skill_Id}")]
        //public async Task<IActionResult> AddPersonHasSkillById(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        //{
        //    var result = await this._person_Has_Need_Skill_Service.AddPersonHasSkillById(Person_Id, Skill_Id, cancellationToken);    
        //    if(result < 0)
        //    {
        //        return BadRequest();       
        //    }
        //    return Ok(result);
        //}

        //[HttpDelete]
        //[Route("/api/[controller]/PersonHasSkill/person={Person_Id}/skill={Skill_Id}")]
        //public async Task<IActionResult> DeletePersonHasSkill(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        //{
        //    var result = await this._person_Has_Need_Skill_Service.DeletePersonHasSkillById(Person_Id, Skill_Id, cancellationToken);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(new SkillFailedResult
        //        {
        //            Errors = result.Errors
        //        });
        //    }
        //    return Ok(result);
        //}

        ////Need skills API

        ///// <summary>
        ///// returns a list of skill a person or user want to acquire.
        ///// ///////
        ///// </summary>
        ///// <param name="Person_Id"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("/api/[controller]/PersonNeedSkill/{Person_Id}")]
        //public async Task<IEnumerable<SkillModel>> GetPersonNeedSkillById(int Person_Id, CancellationToken cancellationToken)
        //{
        //    var result = await this._person_Has_Need_Skill_Service.GetPersonNeedSkillById(Person_Id, cancellationToken);
        //    return result;
        //}

        //[HttpPost]
        //[Route("/api/[controller]/PersonNeedSkill/person={Person_Id}/skill={Skill_Id}")]
        //public async Task<IActionResult> AddPersonNeedSkill(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        //{
        //    var result = await this._person_Has_Need_Skill_Service.AddPersonNeedSkillById(Person_Id, Skill_Id, cancellationToken);
        //    if (result < 0)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(result);
        //}

        //[HttpDelete]
        //[Route("/api/[controller]/PersonNeedSkill/person={Person_Id}/skill={Skill_Id}")]
        //public async Task<IActionResult> DeletePersonNeedSkill(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        //{
        //    var result = await this._person_Has_Need_Skill_Service.DeletePersonNeedSkillById(Person_Id, Skill_Id, cancellationToken);
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest(new SkillFailedResult
        //        {
        //            Errors = result.Errors
        //        });
        //    }
        //    return Ok(result);
        //}
    }
}
