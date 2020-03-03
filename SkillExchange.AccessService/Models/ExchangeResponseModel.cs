using SkillExchange.AccessService.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Models
{
    public class ExchangeResponseModel
    {
        public int User_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public int Exchange_Id { get; set; }
        public Status Status { get; set; }
        public DateTime Opened_TimeStamp { get; set; }
    }
}
