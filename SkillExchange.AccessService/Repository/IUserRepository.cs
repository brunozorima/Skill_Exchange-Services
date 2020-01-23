using Microsoft.AspNet.Identity;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SkillExchange.AccessService.Repository
{
    public interface IUserRepository<TUser> : IDisposable where TUser : class
    {
        public Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(CancellationToken cancellationToken);
        //User GetUserById(int Id);
        //User Save(User user);
        //User UpdateUserById(User user);
        //void Delete(int Id);
    }
}
