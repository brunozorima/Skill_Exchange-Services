using System;
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
    }
}
