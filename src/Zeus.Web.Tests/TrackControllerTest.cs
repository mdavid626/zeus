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

        [TestMethod]
        public void TestEmptyArray()
        {
            var tracker = Substitute.For<ITracker>();
            var controller = new TrackController(tracker);

            var response = controller.Get(new int[0], TrackedEventType.List);

            tracker.DidNotReceive().Track(Arg.Any<TrackedEvent>());
            Assert.IsTrue(response is BadRequestResult);
        }

        [TestMethod]
        public void TestGetWithoutParams()
        {
            var tracker = Substitute.For<ITracker>();
            var controller = new TrackController(tracker);

            var response = controller.Get(null);

            tracker.DidNotReceive().Track(Arg.Any<TrackedEvent>());
            Assert.IsTrue(response is BadRequestResult);
        }

        [TestMethod]
        public void TestGetWithoutIds()
        {
            var tracker = Substitute.For<ITracker>();
            var controller = new TrackController(tracker);

            var response = controller.Get(null, TrackedEventType.List);

            tracker.DidNotReceive().Track(Arg.Any<TrackedEvent>());
            Assert.IsTrue(response is BadRequestResult);
        }

        [TestMethod]
        public void TestGetWithoutType()
        {
            var tracker = Substitute.For<ITracker>();
            var controller = new TrackController(tracker);

            var response = controller.Get(new [] {1, 2, 3});

            tracker.DidNotReceive().Track(Arg.Any<TrackedEvent>());
            Assert.IsTrue(response is BadRequestResult);
        }

        [TestMethod]
        public void TestGetModelError()
        {
            var tracker = Substitute.For<ITracker>();
            var controller = new TrackController(tracker);
            controller.ModelState.AddModelError("ids", "not valid");

            var response = controller.Get(new [] { 1, 2, 3 }, TrackedEventType.List);

            tracker.DidNotReceive().Track(Arg.Any<TrackedEvent>());
            Assert.IsTrue(response is BadRequestResult);
        }
    }
}
