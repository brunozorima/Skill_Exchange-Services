using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Domain
{
    public class AuthSuccessResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Education { get; set; }
        public string WorkExperience { get; set; }
        public int PhotoId { get; set; }
        public string Token { get; set; }
    }
}
