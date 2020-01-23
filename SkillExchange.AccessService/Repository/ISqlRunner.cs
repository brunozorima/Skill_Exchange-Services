using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Repository
{
    public interface ISqlRunner
    {
        IEnumerable<T> Query<T>(IDbConnection connection, string command, object parameters);
        void Execute(IDbConnection connection, string command, object parameters);
    }
}
