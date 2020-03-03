using SkillExchange.AccessService.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Models
{
    public class ExchangeResultModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Exchange_Id { get; set; }
        public string Body { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
