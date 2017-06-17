using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private string sqlConnectionString;

        public ConnectionStringProvider()
        {
            sqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"]?.ConnectionString;
        }

        public string GetSqlConnectionString()
        {
            return sqlConnectionString;
        }
    }
}
