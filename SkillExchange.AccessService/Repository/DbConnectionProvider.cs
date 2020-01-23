using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SkillExchange.AccessService.Repository
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment env;

        //set up the necessary fields to access data
        public DbConnectionProvider(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            this.env = env;
            this.ConnectionString = this._configuration.GetSection("Database").GetValue<string>("ConnectionString");
        }

        public string ConnectionString { get; set; }

        //connect to the database
        public IDbConnection GetConnection()
        {
            if (env.IsProduction())
            {
                var connection = new SqlConnection(_configuration.GetConnectionString("MyDbConnection"));
                //connection.AccessToken = (new AzureServiceTokenProvider()).GetAccessTokenAsync(@"https://database.windows.net/").Result;
                return connection;
            }
            else
            {
                var connection = new SqlConnection(this.ConnectionString);
                return connection;
            }           
        }

        public string GetConnectionString()
        {
            return this.ConnectionString;
        }
    }
}
