using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var tracker = new SqlTracker(ConnectionString);
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
            var tracker = new SqlTracker(ConnectionString);
            var stats = tracker.GetStatistics(DateTime.Now).ToList();
        }
    }
}
