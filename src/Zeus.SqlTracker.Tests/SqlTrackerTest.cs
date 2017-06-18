using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Zeus.Trackers;

namespace Zeus.SqlTrac2ker2.Tests
{
    [TestClass]
    public class SqlTrackerTest
    {
        public const string ConnectionString = "Server=localhost;Database=zeus;Trusted_connection=True";

        [TestMethod]
        public void TestTrackEvent()
        {
            // arrange
            var connStringProvider = Substitute.For<IConnectionStringProvider>();
            connStringProvider.GetSqlConnectionString().Returns(s => ConnectionString);
            var tracker = new SqlTracker(connStringProvider);
            var date = DateTime.Now;

            // act
            tracker.Track(new TrackedEvent()
            {
                Ids = new [] {1, 2, 3},
                Type = TrackedEventType.List,
                CreationDate = date
            });

            // assert
            var query = 
                @"select count(*) as TrackedEventCount
                  from TrackedEvent
                  where ProductId = @productId and EventType = @eventType and EventDate = @eventDate";

            using (var cn = new SqlConnection(ConnectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@productId", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@eventType", SqlDbType.TinyInt).Value = TrackedEventType.List;
                    cmd.Parameters.Add("@eventDate", SqlDbType.DateTime2).Value = date;

                    var reader = cmd.ExecuteReader();

                    var result = reader.Read();
                    Assert.IsTrue(result);

                    var count = (int)reader["TrackedEventCount"];
                    Assert.IsTrue(count == 1);

                    reader.Close();
                }
                cn.Close();
            }
        }

        [TestMethod]
        public void TestGetStatistics()
        {
            var connStringProvider = Substitute.For<IConnectionStringProvider>();
            connStringProvider.GetSqlConnectionString().Returns(s => ConnectionString);
            var tracker = new SqlTracker(connStringProvider);
            var stats = tracker.GetStatistics(DateTime.Now).ToList();
        }
    }
}

