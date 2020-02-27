using SkillExchange.AccessService.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Domain.ExchangeDomain
{
    public class ExchangeResultResponse
    {
        public int Sender_Id { get; set; }
        public int Recipient_Id { get; set; }
        public Status Status { get; set; }
    }
}
