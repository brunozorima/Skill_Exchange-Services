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
    //controller name  = person
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPerson_Has_Need_Skill_Service _person_Has_Need_Skill_Service;
        private readonly ISkillService _skillService;
        public PersonController(IPerson_Has_Need_Skill_Service person_Has_Need_Skill_Service, ISkillService skillService)
        {
            this._person_Has_Need_Skill_Service = person_Has_Need_Skill_Service;
            this._skillService = skillService;
        }

        /// <summary>
        /// ////////
        /// </summary>
        /// <param name="Person_Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //People has skill api
        [HttpGet]
        [Route("/api/[controller]/{person_id}/ownSkills")]
        public async Task<IEnumerable<SkillModel>> GetPersonHasSkillById(int person_id, CancellationToken cancellationToken)
        {
            var result = await this._person_Has_Need_Skill_Service.GetPersonHasSkillById(person_id, cancellationToken);
            return result.OrderBy(skill => skill.Name);
        }

        [HttpPost]
        [Route("/api/[controller]/{Person_Id}/ownSkills/{Skill_Id}")]
        public async Task<IActionResult> AddPersonHasSkillById(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        {
            var result = await this._person_Has_Need_Skill_Service.AddPersonHasSkillById(Person_Id, Skill_Id, cancellationToken);
            if (result < 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("/api/[controller]/{Person_Id}/ownSkills/{Skill_Id}")]
        public async Task<IActionResult> DeletePersonHasSkill(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        {
            var result = await this._person_Has_Need_Skill_Service.DeletePersonHasSkillById(Person_Id, Skill_Id, cancellationToken);
            if (!result.Succeeded)
            {
                return BadRequest(new SkillFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result);
        }


        //Need skills API

        /// <summary>
        /// returns a list of skill a person or user want to acquire.
        /// ///////
        /// </summary>
        /// <param name="Person_Id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/[controller]/{Person_Id}/wantedSkills")]
        public async Task<IEnumerable<SkillModel>> GetPersonNeedSkillById(int Person_Id, CancellationToken cancellationToken)
        {
            var result = await this._person_Has_Need_Skill_Service.GetPersonNeedSkillById(Person_Id, cancellationToken);
            return result.OrderBy(skill => skill.Name); ;
        }

        [HttpPost]
        [Route("/api/[controller]/{Person_Id}/wantedSkills/{Skill_Id}")]
        public async Task<IActionResult> AddPersonNeedSkill(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        {
            var result = await this._person_Has_Need_Skill_Service.AddPersonNeedSkillById(Person_Id, Skill_Id, cancellationToken);
            if (result < 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("/api/[controller]/{Person_Id}/wantedSkills/{Skill_Id}")]
        public async Task<IActionResult> DeletePersonNeedSkill(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        {
            var result = await this._person_Has_Need_Skill_Service.DeletePersonNeedSkillById(Person_Id, Skill_Id, cancellationToken);
            if (!result.Succeeded)
            {
                return BadRequest(new SkillFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/{person_id}")]
        public async Task<IActionResult> GetUserSkillData(int person_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetUserSkillDataAsync(person_id, cancellationToken);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not User Found!");
        }
        [HttpGet]
        [Route("/api/[controller]")]
        public async Task<IActionResult> GetAllUsersSkills(CancellationToken cancellationToken)
        {
            var result = await this._skillService.GetAllserSkillDataAsync(cancellationToken);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not Users Found!");
        }
        [HttpGet]
        [Route("/api/[controller]/{person_id}/skills")]
        public async Task<IActionResult> ShowUsersWithMatchingSkillsAsync(int person_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.ShowUsersWithMatchingSkills(person_id, cancellationToken);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not Users Found!");
        }
        [HttpGet]
        [Route("/api/[controller]/{person_id}/skills/have")]
        public async Task<IActionResult> ShowUsersWithSkillsHaveAsync(int person_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.ShowUsersWithSkillsHave(person_id, cancellationToken);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not Users With the Skills you Have Found!");
        }
        [HttpGet]
        [Route("/api/[controller]/{person_id}/skills/want")]
        public async Task<IActionResult> ShowUsersWithSkillsWantAsync(int person_id, CancellationToken cancellationToken)
        {
            var result = await this._skillService.ShowUsersWithSkillsWant(person_id, cancellationToken);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Not Users With the Skills you Want Found!");
        }
    }
}
