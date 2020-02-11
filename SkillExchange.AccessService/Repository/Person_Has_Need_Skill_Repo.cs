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
    public class Person_Has_Need_Skill_Repo : IPerson_Has_Need_Skill_Repo
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;
        public Person_Has_Need_Skill_Repo(IDbConnectionProvider dbConnectionProvider)
        {
            this._dbConnectionProvider = dbConnectionProvider;
        }
        public async Task<IdentityResult> Add_Person_Has_Skills_By_Id_Async(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.QuerySingleAsync<int>($@"INSERT INTO [Person_Has_Skill] ([Person_Id], [Skill_Id])
                VALUES (@{nameof(Person_Id)}, @{nameof(Skill_Id)})", new { Person_Id, Skill_Id });
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> Add_Person_Need_Skills_By_Id_Async(int Person_Id, int Skill_Id, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.QuerySingleAsync<int>($@"INSERT INTO [Person_Need_Skill] ([Person_Id], [Skill_Id])
                VALUES (@{nameof(Person_Id)}, @{nameof(Skill_Id)})", new { Person_Id, Skill_Id });
            }
            return IdentityResult.Success;
        }

        public async Task<IEnumerable<SkillModel>> Get_Person_Has_Skills_By_Id_Async(int Person_Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QueryAsync<SkillModel>(
                    $@"SELECT [S].[Id], [S].[Name], [S].[Category]
                    FROM [Skill] [S] 
                    JOIN [Person_Has_Skill] [PHS] ON [S].Id = [PHS].[Skill_Id]
                    WHERE [PHS].[Person_Id] = @{nameof(Person_Id)}", new { Person_Id });
            }
        }

        public async Task<IEnumerable<SkillModel>> Get_Person_Need_Skills_By_Id_Async(int Person_Id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
            {
                await connection.OpenAsync(cancellationToken);
                return await connection.QueryAsync<SkillModel>(
                    $@"SELECT [S].[Id], [S].[Name], [S].[Category]
                    FROM [Skill] [S] 
                    JOIN [Person_Need_Skill] [PNS] ON [S].Id = [PNS].[Skill_Id]
                    WHERE [PNS].[Person_Id] = @{nameof(Person_Id)}", new { Person_Id });
            }
        }
    }
}
