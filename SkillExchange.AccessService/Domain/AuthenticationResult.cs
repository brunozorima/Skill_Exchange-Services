using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Domain
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public AuthSuccessResponse authSuccessResponse { get; set; }
    }
}
