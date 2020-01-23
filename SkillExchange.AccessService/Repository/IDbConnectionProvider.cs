using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SkillExchange.AccessService.Repository
{
    public interface IDbConnectionProvider
    {
        IDbConnection GetConnection();

        String GetConnectionString();
    }
}
