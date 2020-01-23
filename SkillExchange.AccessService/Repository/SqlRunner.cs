using Dapper;
using System.Collections.Generic;
using System.Data;

namespace SkillExchange.AccessService.Repository
{
    public class SqlRunner : ISqlRunner
    {
        public void Execute(IDbConnection connection, string command, object parameters)
        {
            connection.Execute(command, parameters, commandType: CommandType.Text);
        }

        public IEnumerable<T> Query<T>(IDbConnection connection, string command, object parameters)
        {
            return connection.Query<T>(command, parameters, commandType: CommandType.Text);
        }
    }
}
