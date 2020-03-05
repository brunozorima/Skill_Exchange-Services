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
    //controller name  = Exchange
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
        public async Task<IActionResult> RequestExchangeAsync([FromBody] ExchangeRequest exchangeRequest, CancellationToken cancellationToken)
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
        public async Task<IActionResult> SendRequestExchangeMessageAsync([FromBody] ExchangeMessage exchangeMessage, CancellationToken cancellationToken)
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
        [Route("/api/[controller]/{exchange_id}/message/{loggedInUser}")]
        public async Task<IActionResult> GetExchangeMessageAsync(int loggedInUser, int exchange_id, CancellationToken cancellationToken)
        {
            var result = await this._exchangeService.GetAllMessagesInOneExchangeAsync(loggedInUser, exchange_id, cancellationToken);
            if (!result.Success)
            {
                return BadRequest(new ExchangeFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.ExchangeResultResponse);
        }

        [HttpGet]
        [Route("/api/[controller]/user/{sender_id}/sent/{status:int?}")]
        public async Task<IActionResult> RequestSentToAsync(int sender_id, CancellationToken cancellationToken, int status = 0)
        {
            var result = await this._exchangeService.RequestSentToAsync(sender_id, cancellationToken, status);
            if (!result.Success)
            {
                return BadRequest(new ExchangeFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.ExchangeObjectUserModel);
        }

        [HttpGet]
        [Route("/api/[controller]/user/{recipient_id}/recieved/{status:int?}")]
        public async Task<IActionResult> RequestRecievedFromAsync(int recipient_id, CancellationToken cancellationToken, int status = 0)
        {
            var result = await this._exchangeService.RequestRecievedFromAsync(recipient_id, cancellationToken, status);
            if (!result.Success)
            {
                return BadRequest(new ExchangeFailedResult
                {
                    Errors = result.Errors
                });
            }
            return Ok(result.ExchangeObjectUserModel);
        }
    }
}
