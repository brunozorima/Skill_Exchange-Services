using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SkillExchange.AccessService.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
    

namespace SkillExchange.AccessService.Repository
{
    public class UserStore : IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPhoneNumberStore<ApplicationUser>,
    IUserTwoFactorStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserRepository<ApplicationUser>
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public UserStore(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                user.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [ApplicationUser] ([UserName], [FirstName], [LastName], [NormalizedUserName], [Email],
                [NormalizedEmail], [EmailConfirmed], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled])
                VALUES (@{nameof(ApplicationUser.UserName)}, @{nameof(ApplicationUser.FirstName)}, @{nameof(ApplicationUser.LastName)}, @{nameof(ApplicationUser.NormalizedUserName)}, @{nameof(ApplicationUser.Email)},
                @{nameof(ApplicationUser.NormalizedEmail)}, @{nameof(ApplicationUser.EmailConfirmed)}, @{nameof(ApplicationUser.PasswordHash)},
                @{nameof(ApplicationUser.PhoneNumber)}, @{nameof(ApplicationUser.PhoneNumberConfirmed)}, @{nameof(ApplicationUser.TwoFactorEnabled)});
                SELECT CAST(SCOPE_IDENTITY() as int)", user);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [ApplicationUser] WHERE [Id] = @{nameof(ApplicationUser.Id)}", user);
            }

            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM [ApplicationUser]
                WHERE [Id] = @{nameof(userId)}", new { userId });
            }
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM [ApplicationUser]
                WHERE [NormalizedUserName] = @{nameof(normalizedUserName)}", new { normalizedUserName });
            }
        }

        //Get all users
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<ApplicationUser>($@"SELECT [Id], [UserName], [FirstName], [LastName], [Email] FROM [ApplicationUser]");
                return result.ToList();
            }
        }
        //Get a user based on Id
        public async Task<ApplicationUser> GetUserdById(int user_id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT [Id], [UserName], [FirstName], [LastName], [Email] FROM [ApplicationUser]
                WHERE [Id] = @{nameof(user_id)}", new { user_id });
            }
        }
        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
         {
            return Task.FromResult(user.NormalizedUserName);
         }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            //todo: implement password change logic
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [ApplicationUser] SET
                [UserName] = @{nameof(ApplicationUser.UserName)},
                [FirstName] = @{nameof(ApplicationUser.FirstName)},
                [LastName] = @{nameof(ApplicationUser.LastName)},
                [NormalizedUserName] = @{nameof(ApplicationUser.NormalizedUserName)},
                [Email] = @{nameof(ApplicationUser.Email)},
                [NormalizedEmail] = @{nameof(ApplicationUser.NormalizedEmail)},
                [EmailConfirmed] = @{nameof(ApplicationUser.EmailConfirmed)},             
                [PhoneNumber] = @{nameof(ApplicationUser.PhoneNumber)},
                [PhoneNumberConfirmed] = @{nameof(ApplicationUser.PhoneNumberConfirmed)},
                [TwoFactorEnabled] = @{nameof(ApplicationUser.TwoFactorEnabled)}
                WHERE [Id] = @{nameof(ApplicationUser.Id)}", user);
            }
            ////[PasswordHash] = @{nameof(ApplicationUser.PasswordHash)},
            return IdentityResult.Success;
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM [ApplicationUser]
                WHERE [NormalizedEmail] = @{nameof(normalizedEmail)}", new { normalizedEmail });
            }
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

    }
}
