using System;
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
            var connStringProvider = Substitute.For<IConnectionStringProvider>();
            connStringProvider.GetSqlConnectionString().Returns(s => ConnectionString);
            var tracker = new SqlTracker(connStringProvider);
            tracker.Track(new TrackedEvent()
            {
                Ids = new [] {1, 2, 3},
                Type = TrackedEventType.List,
                CreationDate = DateTime.Now
            });
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
