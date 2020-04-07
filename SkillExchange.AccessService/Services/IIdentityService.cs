using SkillExchange.AccessService.Domain;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Services
{
    public interface IIdentityService
    {
        public Task<AuthenticationResult> RegisterAsync(User user);
        public Task<AuthenticationResult> LoginAsync(LoginModel user);
        public Task<IEnumerable<ApplicationUser>> ListUsers(CancellationToken cancellationToken);
        public Task<AuthenticationResult> DeleteAsync(int id);
        public Task<AuthenticationResult> GetUserByIdAsync(int id);
        public Task<AuthenticationResult> UpdateUserByIdAsync(User user);
        public Task<ApplicationUser> GetUserById(int id, CancellationToken cancellationToken);
    }
}
