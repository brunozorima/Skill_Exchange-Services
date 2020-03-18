using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Domain.ExchangeDomain
{
    public class ExchangeResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public IEnumerable<ExchangeResultModel> ExchangeResultResponse { get; set; }
        public IEnumerable<ExchangeResponseModel> ExchangeObjectUserModel { get; set; }
        public int Returned_Id { get; set; }
        public ExchangeResultModel ExchangeMessage { get; set; }
    }
}
