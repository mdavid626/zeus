using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Zeus.Trackers;

namespace Zeus.Trackers.Tests
{
    [TestClass]
    public class SqlTrackerTest
    {
        [TestMethod]
        public void TestTrackEvent()
        {
            // arrange
            var dbContextProvider = Substitute.For<ITrackedEventContextProvider>();
            using (var connection = Effort.DbConnectionFactory.CreateTransient())
            {
                dbContextProvider.Provide().Returns(p => new TrackedEventContext(connection, false));
                var tracker = new SqlTracker(dbContextProvider);
                var date = DateTime.Now;

                // act
                tracker.Track(new TrackedEvent()
                {
                    Ids = new[] { 1, 2, 3 },
                    Type = TrackedEventType.List,
                    CreationDate = date
                });

                // assert
                using (var ctx = dbContextProvider.Provide())
                {
                    var any = ctx.TrackedEvents
                        .Any(t => t.ProductId == 1 && t.EventType == (byte) TrackedEventType.List && t.EventDate == date);
                    Assert.IsTrue(any);
                }
            }
        }

        [TestMethod]
        public void TestGetStatistics()
        {
            // arrange
            var dbContextProvider = Substitute.For<ITrackedEventContextProvider>();
            using (var connection = Effort.DbConnectionFactory.CreateTransient())
            {
                dbContextProvider.Provide().Returns(p => new TrackedEventContext(connection, false));
                var tracker = new SqlTracker(dbContextProvider);

                var date = DateTime.Now.AddDays(-1);

                tracker.Track(new TrackedEvent()
                {
                    Ids = new [] { 1 },
                    Type = TrackedEventType.List,
                    CreationDate = date
                });

                // act
                var stats = tracker.GetStatistics(DateTime.Now).ToList();
                var item = stats.Single();
                
                // assert
                Assert.IsTrue(item.Id == 1);
                Assert.IsTrue(item.ListImpressions == 1);
                Assert.IsTrue(item.DetailsViews == 0);
                Assert.IsTrue(item.Conversions == 0);
                Assert.IsTrue(item.ClickRate7Days == 0);
                Assert.IsTrue(item.ConversionRate7Days == 0);
                Assert.IsTrue(item.ConversionRate14Days == 0);
            }
        }
    }
}



