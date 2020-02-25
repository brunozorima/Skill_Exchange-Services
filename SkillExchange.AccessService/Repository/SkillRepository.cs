﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNet.Identity;
using SkillExchange.AccessService.Models;

namespace SkillExchange.AccessService.Repository
{
    public class SkillRepository : ISkillRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public SkillRepository(IDbConnectionProvider dbConnectionProvider)
        {
            this._dbConnectionProvider = dbConnectionProvider;
        }

        public async Task<SkillModel> AddSkill(SkillModel skillModel, CancellationToken cancellationToken)
        {
            SkillModel skillModelResult = null;
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                skillModelResult = await connection.QuerySingleAsync<SkillModel>($@"INSERT INTO [Skill] ([Id], [Name], [Category],                
                VALUES (@{nameof(SkillModel.Id)}, @{nameof(SkillModel.Name)}, @{nameof(SkillModel.Category)});
                SELECT CAST(SCOPE_IDENTITY() as int)", skillModel);
            }
            return skillModelResult;
        }

        public async Task<IdentityResult> DeleteSkill(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [Skill] WHERE [Id] = @{nameof(SkillModel.Id)}", id);
            }
            return IdentityResult.Success;
        }

        public async Task<IEnumerable<SkillModel>> GetAllSkillsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<SkillModel>($@"SELECT [Id], [Name], [Category] FROM [Skill]");
                return result.ToList();
            }
        }

        public async Task<IEnumerable<SkillModel>> GetSkillByCategory(int category_id, CancellationToken cancellationToken)
        {
            var category = Enum.GetName(typeof(Category), category_id);
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<SkillModel>($@"SELECT [Id], [Name], [Category] FROM [Skill] WHERE [Category] = @{nameof(category)}", new { category });
                return result.ToList();
            }
        }

        public async Task<SkillModel> GetSkillById(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QuerySingleOrDefaultAsync<SkillModel>($@"SELECT [Id], [Name], [Category] FROM [Skill] WHERE [Id] = @{nameof(id)}", new { id });
            }
        }
        public async Task<IEnumerable<SkillModel>> FindSkillByName(string name, CancellationToken cancellationToken)
        {
            string sql = "SELECT [Id], [Name], [Category] FROM [Skill] WHERE [Name] LIKE CONCAT('%',@name,'%')";
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<SkillModel>(sql, new { name });
                return result.ToList();
            }
        }
        /// <summary>
        /// /this returns a list of people who wants to gain this spefic skills set
        /// </summary>
        /// <param name="skill_id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUser>> GetWantedPersonBySkillId(int skill_id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<ApplicationUser>($@"
                    SELECT [U].[Id], [U].[FirstName], [U].[LastName], [U].[Email]
                    FROM [ApplicationUser] [U]    
                    JOIN [Person_Need_Skill] [PNS] ON [U].Id = [PNS].[Person_Id]
                    WHERE [PNS].[Skill_Id] = @{nameof(skill_id)}", new { skill_id });
                return result.ToList();
            }
        }
        /// <summary>
        /// Returns a list of people who owns this specific skills set
        /// </summary>
        /// <param name="skill_id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationUser>> GetPersonOwningSkillsBySkillId(int skill_id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<ApplicationUser>($@"
                    SELECT [U].[Id], [U].[FirstName], [U].[LastName], [U].[Email]
                    FROM [ApplicationUser] [U]    
                    JOIN [Person_Has_Skill] [PHS] ON [U].Id = [PHS].[Person_Id]
                    WHERE [PHS].[Skill_Id] = @{nameof(skill_id)}", new { skill_id });
                return result.ToList();
            }
        }
    }
}
