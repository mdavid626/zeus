using System;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Zeus.Trackers;
using Zeus.Web.Controllers;

namespace Zeus.Web.Tests
{
    [TestClass]
    public class TrackControllerTest
    {
        [TestMethod]
        public void TestGet()
        {
            var ids = new[] {1, 2, 3};
            var type = TrackedEventType.List;
            var tracker = Substitute.For<ITracker>();

            var controller = new TrackController(tracker);
            var response = controller.Get(ids, type);

            tracker.Received().Track(Arg.Is<TrackedEvent>(t => t.Ids.SequenceEqual(ids) && t.Type == type));
            Assert.IsTrue(response is OkResult);
        }
    }
}
