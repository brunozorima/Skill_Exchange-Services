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

        public async Task<int> AddSkill(SkillModel skillModel, CancellationToken cancellationToken)
        {
            var category = (int)skillModel.Category;
            var skillName = skillModel.Name;

            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                skillModel.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO [Skill] ([Name], [Category])                
                VALUES (@{nameof(skillName)}, @{nameof(category)});
                SELECT CAST(SCOPE_IDENTITY() as int)", new { skillName, category });
            }
            return skillModel.Id;
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
            //var category = Enum.GetName(typeof(Category), category_id);
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<SkillModel>($@"SELECT [Id], [Name], [Category] FROM [Skill] WHERE [Category] = @{nameof(category_id)}", new { category_id });
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
        //GET ALL THE PEOPLE THAT HAVE THE SKILLS I NEED AND THE PEOPLE NEEDS THE SKILLS I HAVE AND MATCH ONLY THOSE WITH EXCHANGABLE SKILLS
        public async Task<IEnumerable<int>> GetAutoMatch(int loggedInUser, CancellationToken cancellationToken)
        {
            var sql = "SELECT WantedSkills.wants, haveSkills.has FROM " +
                "(SELECT Person_Id as wants, count(*) as _countHave FROM [Person_Need_Skill] " +
                "WHERE Skill_Id " +
                "IN(SELECT Skill_Id from [Person_Has_Skill] WHERE Person_Id = @loggedInUser) " +
                "Group By Person_Id " +
                "Having COUNT(Person_Id) > 0) AS WantedSkills, " +
                "(SELECT Person_Id AS has, count(Person_Id) as _countWant from [Person_Has_Skill] WHERE Skill_Id " +
                "IN(SELECT Skill_Id FROM [Person_Need_Skill] WHERE Person_Id = @loggedInUser) " +
                "Group By Person_Id " +
                "Having COUNT(Person_Id) > 0) as haveSkills " +
                "WHERE WantedSkills.wants = haveSkills.has " +
                "AND WantedSkills._countHave > 0 " +
                "AND haveSkills._countWant > 0";

            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<int>(sql, new { loggedInUser });
                return result.ToList();
            }
        }
        //GET ALL THE PEOPLE THAT HAVE THE SKILLS I WANT
        public async Task<IEnumerable<int>> GetPeopleWithSkillsWant(int loggedInUser, CancellationToken cancellationToken)
        {
            var sql = "SELECT WantedSkills.wants FROM " +
                "(SELECT Person_Id as wants FROM [Person_Need_Skill] " +
                "WHERE Skill_Id " +
                "IN(SELECT Skill_Id from [Person_Has_Skill] WHERE Person_Id = @loggedInUser) " +
                "Group By Person_Id " +
                "Having COUNT(Person_Id) > 0) AS WantedSkills";

            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<int>(sql, new { loggedInUser });
                return result.ToList();
            }
        }
        //GET ALL THE PEOPLE THAT NEEDS THE SKILLS I HAVE
        public async Task<IEnumerable<int>> GetPeopleWithSkillsHave(int loggedInUser, CancellationToken cancellationToken)
        {
            var sql = "SELECT haveSkills.has FROM " +
                "(SELECT Person_Id AS has FROM [Person_Has_Skill] WHERE Skill_Id " +
                "IN(SELECT Skill_Id FROM [Person_Need_Skill] WHERE Person_Id = @loggedInUser) " +
                "Group By Person_Id " +
                "Having COUNT(Person_Id) > 0) AS haveSkills";

            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                var result = await connection.QueryAsync<int>(sql, new { loggedInUser });
                return result.ToList();
            }
        }

    }
}
