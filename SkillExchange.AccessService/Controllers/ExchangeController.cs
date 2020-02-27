using Microsoft.AspNetCore.Mvc;
using SkillExchange.AccessService.Domain.ExchangeDomain;
using SkillExchange.AccessService.Models;
using SkillExchange.AccessService.Services.ExchangeService;
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
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }


        [HttpPost]
        [Route("/api/[controller]/request")]
        public async Task<IActionResult> RequestExchange([FromBody] ExchangeRequest exchangeRequest, CancellationToken cancellationToken)
        {
            var result = await this._exchangeService.RequestExchangeAsync(exchangeRequest, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(new ExchangeFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.Returned_Id);
        }

        [HttpPost]
        [Route("/api/[controller]/message")]
        public async Task<IActionResult> SendRequestExchangeMessage([FromBody] ExchangeMessage exchangeMessage, CancellationToken cancellationToken)
        {
            var result = await this._exchangeService.SendExchangeMessageAsync(exchangeMessage, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(new ExchangeFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.Returned_Id);
        }

        [HttpGet]
        [Route("/api/[controller]/{recipient_id}/request/{status}")]
        public async Task<IActionResult> GetRequestExchange(int recipient_id, int status, CancellationToken cancellationToken)
        {
            var result = await this._exchangeService.GetRequestListAsync(recipient_id, status, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(new ExchangeFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.requestedUser);
        }
    }
}
