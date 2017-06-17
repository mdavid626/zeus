using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    public class SqlTracker : ITracker
    {
        public string ConnectionString { get; }

        public SqlTracker()
        {
            
        }

        public SqlTracker(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Track(TrackedEvent trackedEvent)
        {
            string query = "insert into [TrackedEvent] ([ProductId], [EventType], [EventDate]) " +
                           "values (@ProductId, @EventType, @EventDate)";

            using (var cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                foreach (var id in trackedEvent.Ids)
                {
                    using (var cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("@EventType", SqlDbType.TinyInt).Value = trackedEvent.Type;
                        cmd.Parameters.Add("@EventDate", SqlDbType.DateTime2).Value = trackedEvent.CreationDate;
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                cn.Close();
            }
        }

        public IEnumerable<ProductStatistics> GetStatistics(DateTime upperBound)
        {
            var query = @"
            select 
                s.ProductId,
                s.ListImpressions,
                s.DetailsViews,
                s.Conversions,
                s.ListViews7Days,
                s.DetailViews7Days,
                s.ConversionViews7Days,
                s.DetailViews14Days,
                s.ConversionViews14Days
            from (
            select
                t.ProductId,
                -- # List Impressions
                (select count(*)
                 from TrackedEvent as t2
                 where t2.ProductId = t.ProductId and t2.EventType = 1 and
                       t2.EventDate >= dateadd(day, -1, @upperBound) and
                       t2.EventDate < @upperBound) as ListImpressions,
                -- # Details Views
                (select count(*)
                 from TrackedEvent as t2
                 where t2.ProductId = t.ProductId and t2.EventType = 2 and
                       t2.EventDate >= dateadd(day, -1, @upperBound) and
                       t2.EventDate < @upperBound) as DetailsViews,
                -- # Conversions
                (select count(*)
                 from TrackedEvent as t2
                 where t2.ProductId = t.ProductId and t2.EventType = 3 and
                       t2.EventDate >= dateadd(day, -1, @upperBound) and
                       t2.EventDate < @upperBound) as Conversions,
                -- ListViews7Days
                (select count(*)
                 from TrackedEvent as t2
                 where t2.ProductId = t.ProductId and t2.EventType = 1 and
                       t2.EventDate >= dateadd(day, -7, @upperBound) and
                       t2.EventDate < @upperBound) as ListViews7Days,
                -- DetailViews7Days
                (select count(*)
                 from TrackedEvent as t2
                 where t2.ProductId = t.ProductId and t2.EventType = 2 and
                       t2.EventDate >= dateadd(day, -7, @upperBound) and
                       t2.EventDate < @upperBound) as DetailViews7Days,
                -- ConversionViews7Days
                (select count(*)
                 from TrackedEvent as t2
                 where t2.ProductId = t.ProductId and t2.EventType = 3 and
                       t2.EventDate >= dateadd(day, -7, @upperBound) and
                       t2.EventDate < @upperBound) as ConversionViews7Days,
                -- DetailViews14Days
                (select count(*)
                 from TrackedEvent as t2
                 where t2.ProductId = t.ProductId and t2.EventType = 2 and
                       t2.EventDate >= dateadd(day, -14, @upperBound) and
                       t2.EventDate < @upperBound) as DetailViews14Days,
                -- ConversionViews14Days
                (select count(*)
                 from TrackedEvent as t2
                 where t2.ProductId = t.ProductId and t2.EventType = 3 and
                       t2.EventDate >= dateadd(day, -14, @upperBound) and
                       t2.EventDate < @upperBound) as ConversionViews14Days
            from TrackedEvent as t
            group by t.ProductId) as s
            where
                s.ListImpressions > 0 or s.DetailsViews > 0 or
                s.Conversions > 0 or s.ListViews7Days > 0 or
                s.DetailViews7Days > 0 or s.ConversionViews7Days > 0 or
                s.DetailViews14Days > 0 or ConversionViews14Days > 0";

            using (var cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@upperBound", SqlDbType.DateTime2).Value = upperBound.Date;

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        yield return ReaderToStatistics(reader);
                    }
                    reader.Close();
                }
                cn.Close();
            }
        }

        private ProductStatistics ReaderToStatistics(SqlDataReader reader)
        {
            var listViews7Days = (int)reader["ListViews7Days"];
            var detailViews7Days = (int)reader["DetailViews7Days"];
            var conversionViews7Days = (int)reader["ConversionViews7Days"];
            var detailViews14Days = (int)reader["DetailViews14Days"];
            var conversionViews14Days = (int)reader["ConversionViews14Days"];

            var clickRate7Days = listViews7Days != 0
                ? Math.Min(Math.Round(100.0 * detailViews7Days / listViews7Days, 2), 100)
                : 0;
            var conversionRate7Days = detailViews7Days != 0
                ? Math.Min(Math.Round(100.0 * conversionViews7Days / detailViews7Days, 2), 100)
                : 0;
            var conversionRate14Days = detailViews14Days != 0
                ? Math.Min(Math.Round(100.0 * conversionViews14Days / detailViews14Days, 2), 100)
                : 0;

            return new ProductStatistics()
            {
                Id = (int)reader["ProductId"],
                ListImpressions = (int)reader["ListImpressions"],
                DetailsViews = (int)reader["DetailsViews"],
                Conversions = (int)reader["Conversions"],
                ClickRate7Days = clickRate7Days,
                ConversionRate7Days = conversionRate7Days,
                ConversionRate14Days = conversionRate14Days
            };
        }
    }
}
