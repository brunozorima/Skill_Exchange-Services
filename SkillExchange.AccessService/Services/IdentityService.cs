using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillExchange.AccessService.Domain;
using SkillExchange.AccessService.Models;
using SkillExchange.AccessService.Repository;

namespace SkillExchange.AccessService.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository<ApplicationUser> _userRepository;
        public IdentityService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUserRepository<ApplicationUser> userRepository)
        {
            this._userManager = userManager;
            this._configuration = configuration;      
            this._userRepository = userRepository;
        }

        public async Task<AuthenticationResult> RegisterAsync(User user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User already exists!" }
                };
            }

            var newUser = new ApplicationUser
            {
                Email = user.Email,
                UserName = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName
            };

            var createdUser = await _userManager.CreateAsync(newUser, user.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(result => result.Description)
                };
            }

            return GenerateAuthenticationResultForTheUser(newUser);
        }
        public async Task<AuthenticationResult> LoginAsync(LoginModel user)
        {
            var _user = await _userManager.FindByEmailAsync(user.Email);

            if (_user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exists!" }
                };
            }

            var userHasValidPassword = await this._userManager.CheckPasswordAsync(_user, user.Password);
            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Invalid login!" }
                };
            }

            return GenerateAuthenticationResultForTheUser(_user);
        }

        private AuthenticationResult GenerateAuthenticationResultForTheUser(ApplicationUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this._configuration.GetSection("Token").GetValue<string>("Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var authResponse = new AuthSuccessResponse
            {
                Id = newUser.Id,
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Password = null,
                Token = tokenHandler.WriteToken(token)
            };
    
            return new AuthenticationResult
            {                
                Success = true,    
                authSuccessResponse = authResponse
            };
        }

        public async Task<AuthenticationResult> DeleteAsync(int id)
        {
            //get the user
            var isExistingUser = await this._userManager.FindByIdAsync(id.ToString());
            //if the user does exist, then delete it
            if (isExistingUser != null)
            {
                await this._userManager.DeleteAsync(isExistingUser);
                return new AuthenticationResult
                {
                    Success = true
                };
            }
            return new AuthenticationResult
            {
                Errors = new[] { "Invalid request! User does not exists!"}
            };
        }

        public async Task<AuthenticationResult> GetUserByIdAsync(int id)
        {
            var user = await this._userManager.FindByIdAsync(id.ToString());
            if(user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exists!" }
                };
            }
            var authResponse = new AuthSuccessResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
            return new AuthenticationResult
            {
                Success = true,
                authSuccessResponse = authResponse
            };

        }

        public async Task<AuthenticationResult> UpdateUserByIdAsync(User user)
        {
            var _user = await this._userManager.FindByIdAsync(user.Id.ToString());
            if(_user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User Does Not Exist!" }
                };
            }

            var updateThisUser = new ApplicationUser
            {   Id = user.Id,
                Email = user.Email,
                UserName = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName
            };
            var updatedUser = await this._userManager.UpdateAsync(updateThisUser);
        
            if (!updatedUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = updatedUser.Errors.Select(x => x.Description).ToList()
                };
            }

            var authResponse = new AuthSuccessResponse
            {
                Id = user.Id,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName
            };
            return new AuthenticationResult
            {
                Success = true,
                authSuccessResponse = authResponse
            };


            //this will be need for future use!
            //if(!await this._userManager.CheckPasswordAsync(_user, user.Password))
            //{
            //    return new AuthenticationResult
            //    {
            //        Errors = new[] { "Update Cannot Succeed! Only the correct user can update this record." }
            //    };
            //}
            //this needs to be used when changing passwords
            //var newPasswordHash = this._userManager.PasswordHasher.HashPassword(_user, user.Password);
        }

        public async Task<IEnumerable<ApplicationUser>> ListUsers(CancellationToken cancellationToken)
        {
            var result = await this._userRepository.GetAllUsersAsync(cancellationToken);
            return result;
        }
    }
}
