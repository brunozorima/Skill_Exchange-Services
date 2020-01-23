using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using SkillExchange.AccessService.Models;

namespace SkillExchange.AccessService.Repository
{
    public class UserRepository
    {
        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        //private readonly IDbConnectionProvider _dbConnectionProvider;
        //private readonly ISqlRunner _sqlRunner;

        //public UserRepository(IDbConnectionProvider dbConnectionProvider, ISqlRunner sqlRunner)
        //{
        //    _dbConnectionProvider = dbConnectionProvider;
        //    _sqlRunner = sqlRunner;
        //}

        //public void Delete(int Id)
        //{
        //    string sql = "DELETE FROM student WHERE Id = @Id";
        //    var con = this._dbConnectionProvider.GetConnection();
        //    this._sqlRunner.Execute(con, sql, new { Id });
        //}

        //public User GetUserById(int Id)
        //{
        //    string sql = "SELECT Id, FirstName, LastName, Email FROM User WHERE Id = @Id";
        //    var con = this._dbConnectionProvider.GetConnection();
        //    return this._sqlRunner.Query<User>(con, sql, new { Id }).FirstOrDefault();
        //}

        //public IEnumerable<User> GetUsers()
        //{
        //    string sql = "SELECT Id, FirstName, LastName, Email FROM User";
        //    var con = new SqlConnection(this._dbConnectionProvider.GetConnectionString());
        //    return this._sqlRunner.Query<User>(con, sql, null);

        //}
        ////Get all users
        //public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        //{
        //    using (var connection = new SqlConnection(this._dbConnectionProvider.GetConnectionString()))
        //    {
        //        string sql = "SELECT id, firstName, lastName FROM ApplicationUser";
        //        await connection.OpenAsync();
        //        var result = await connection.QueryAsync<ApplicationUser>(sql);
        //        return result.ToList();
        //    }
        //}
        //public User Save(User user)
        //{
        //    //SQL statement to save a new user
        //    string sql = "INSERT INTO User (Id, FirstName. LastName, Email, Password) " +
        //        "VALUES (@firstName, @lastName, @email, @password); " +
        //        "SELECT CAST(SCOPE_IDENTITY() as int)";

        //    //connect to the database
        //    var con = this._dbConnectionProvider.GetConnection();

        //    //save and return the Id of the new user
        //    var _user = this._sqlRunner.Query<int>(con, sql,
        //        new
        //        {
        //            firstName = user.FirstName,
        //            lastName = user.LastName,
        //            email = user.Email,
        //            password = user.Password
        //        }).Single();

        //    return GetUserById(_user);
        //}

        //public User UpdateUserById(User user)
        //{
        //    string sql = "UPDATE User " +
        //        "SET firstName = @firstName, lastName = @lastName, email = @email, password = @password WHERE Id = @id";
        //    //connect to the database
        //    var con = this._dbConnectionProvider.GetConnection();
        //    this._sqlRunner.Execute(con, sql,
        //        new
        //        {
        //            id = user.Id,
        //            firstName = user.FirstName,
        //            lastName = user.LastName,
        //            email = user.Email,
        //            password = user.Password
        //        });

        //    return GetUserById(user.Id);
        //}
        //public Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
