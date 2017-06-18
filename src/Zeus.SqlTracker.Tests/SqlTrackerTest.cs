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
        public void TestGetStatisticsList()
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

        [TestMethod]
        public void TestGetStatisticsClickRate()
        {
            // arrange
            var dbContextProvider = Substitute.For<ITrackedEventContextProvider>();
            using (var connection = Effort.DbConnectionFactory.CreateTransient())
            {
                dbContextProvider.Provide().Returns(p => new TrackedEventContext(connection, false));
                var tracker = new SqlTracker(dbContextProvider);

                var date = DateTime.Now.AddDays(-1);
                var date4DaysBefore = DateTime.Now.AddDays(-4);

                tracker.Track(new TrackedEvent()
                {
                    Ids = new[] { 1 },
                    Type = TrackedEventType.List,
                    CreationDate = date
                });

                tracker.Track(new TrackedEvent()
                {
                    Ids = new[] { 1 },
                    Type = TrackedEventType.Details,
                    CreationDate = date4DaysBefore
                });

                tracker.Track(new TrackedEvent()
                {
                    Ids = new[] { 1 },
                    Type = TrackedEventType.Conversion,
                    CreationDate = date4DaysBefore
                });

                // act
                var stats = tracker.GetStatistics(DateTime.Now).ToList();
                var item = stats.Single();

                // assert
                Assert.IsTrue(item.Id == 1);
                Assert.IsTrue(item.ListImpressions == 1);
                Assert.IsTrue(item.DetailsViews == 0);
                Assert.IsTrue(item.Conversions == 0);
                Assert.IsTrue(item.ClickRate7Days == 100);
                Assert.IsTrue(item.ConversionRate7Days == 100);
                Assert.IsTrue(item.ConversionRate14Days == 100);
            }
        }

        [TestMethod]
        public void TestGetStatisticsClickRate50Percent()
        {
            // arrange
            var dbContextProvider = Substitute.For<ITrackedEventContextProvider>();
            using (var connection = Effort.DbConnectionFactory.CreateTransient())
            {
                dbContextProvider.Provide().Returns(p => new TrackedEventContext(connection, false));
                var tracker = new SqlTracker(dbContextProvider);

                var date = DateTime.Now.AddDays(-1);
                var date4DaysBefore = DateTime.Now.AddDays(-4);

                tracker.Track(new TrackedEvent()
                {
                    Ids = new[] { 1 },
                    Type = TrackedEventType.List,
                    CreationDate = date
                });

                tracker.Track(new TrackedEvent()
                {
                    Ids = new[] { 1 },
                    Type = TrackedEventType.List,
                    CreationDate = date4DaysBefore
                });

                tracker.Track(new TrackedEvent()
                {
                    Ids = new[] { 1 },
                    Type = TrackedEventType.Details,
                    CreationDate = date4DaysBefore
                });

                // act
                var stats = tracker.GetStatistics(DateTime.Now).ToList();
                var item = stats.Single();

                // assert
                Assert.IsTrue(item.Id == 1);
                Assert.IsTrue(item.ListImpressions == 1);
                Assert.IsTrue(item.DetailsViews == 0);
                Assert.IsTrue(item.Conversions == 0);
                Assert.IsTrue(item.ClickRate7Days == 50);
                Assert.IsTrue(item.ConversionRate7Days == 0);
                Assert.IsTrue(item.ConversionRate14Days == 0);
            }
        }
    }
}



