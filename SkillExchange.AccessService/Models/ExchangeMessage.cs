using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Models
{
    public class ExchangeMessage
    {
        public int Id { get; set; }
        public int Sender_Id { get; set; }
        public int Recipient_Id { get; set; }
        public string MessageBody { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
