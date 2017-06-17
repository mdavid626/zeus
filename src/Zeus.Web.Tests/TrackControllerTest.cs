using System;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.Trackers;
using Zeus.Web.Controllers;

namespace Zeus.Web.Tests
{
    [TestClass]
    public class TrackControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ctrl = new TrackController(new SqlTracker());
            var response = ctrl.Get(new [] {1, 2, 3}, TrackedEventType.List);
            Assert.IsTrue(response is OkResult);
        }
    }
}
