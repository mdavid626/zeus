using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    public class TrackedEventContext : DbContext
    {
        public TrackedEventContext() : base("SqlConnectionString")
        {
            
        }

        public TrackedEventContext(DbConnection connection, bool contextOwnsConnection) : base(connection, contextOwnsConnection)
        {
            
        }

        public DbSet<TrackedEventDto> TrackedEvents { get; set; }

        static TrackedEventContext()
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, ensures static reference to System.Data.Entity.SqlServer");
        }
    }
}
