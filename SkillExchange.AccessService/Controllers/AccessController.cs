using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SkillExchange.AccessService.Domain;
using SkillExchange.AccessService.Models;
using SkillExchange.AccessService.Services;
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
    public class AccessController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IIdentityService _identityService;
        public AccessController(IIdentityService identityService)
        {
            this._identityService = identityService;
        }

        [HttpPost]
        [Route("/api/[controller]/register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await this._identityService.RegisterAsync(user);
            if (!result.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.authSuccessResponse);
        }


        [HttpPost]
        [Route("/api/[controller]/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel user)
        {
            var result = await this._identityService.LoginAsync(user);
            if (!result.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.authSuccessResponse);
        }

        [HttpDelete]
        [Route("/api/[controller]/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await this._identityService.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = result.Errors
                });
            }
            return Ok();
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await this._identityService.GetUserByIdAsync(id);
            if(!result.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.authSuccessResponse);
        }


        [HttpGet]
        [Route("/api/[controller]/users")]
        public async Task<IEnumerable<ApplicationUser>> ListUsers(CancellationToken cancellationToken)
        {
            var results = await this._identityService.ListUsers(cancellationToken);
            return results.ToList();
        }

        [HttpPut]
        [Route("/api/[controller]/updateUser")]
        public async Task<IActionResult> UpdateUserById([FromBody] ApplicationUser user)
        {
            var result = await this._identityService.UpdateUserByIdAsync(user);
            if (!result.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.authSuccessResponse);
        }

        [HttpGet]
        [Route("/api/[controller]/test")]
        public IEnumerable<string> test()
        {
            return Summaries.ToArray();
        }
    }
}