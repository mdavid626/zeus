using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var response = ctrl.Get();
            Assert.AreEqual(response, String.Empty);
        }
    }
}
