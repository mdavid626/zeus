using System;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.Common;
using Zeus.Web.Controllers;

namespace Zeus.Web.Tests
{
    [TestClass]
    public class TrackControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ctrl = new TrackController();
            var response = ctrl.Get(new [] {1, 2, 3}, TrackedEventType.List);
            Assert.IsTrue(response is OkResult);
        }
    }
}
