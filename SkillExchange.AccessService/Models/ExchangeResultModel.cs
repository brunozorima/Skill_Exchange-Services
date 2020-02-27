using SkillExchange.AccessService.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Models
{
    public class ExchangeResultModel
    {
        public int Id { get; set; }
        public int Sender_Id { get; set; }
        public int Recipient_Id { get; set; }
        public Status Status { get; set; }
        public DateTime Opened_TimeStamp { get; set; }
        public DateTime Last_Message_TimeStamp { get; set; }
    }
}
