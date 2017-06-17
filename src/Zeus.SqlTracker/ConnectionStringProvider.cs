using System;
using System.Collections.Generic;
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
            var settings = new Properties.Settings();
            sqlConnectionString = settings.SqlConnectionString;
        }

        public string GetSqlConnectionString()
        {
            return sqlConnectionString;
        }
    }
}
