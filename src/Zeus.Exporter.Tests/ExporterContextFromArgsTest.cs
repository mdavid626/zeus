using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zeus.Exporter.Tests
{
    [TestClass]
    public class ExporterContextFromArgsTest
    {
        [TestMethod]
        public void TestUpperBoundOk()
        {
            var args = new[] { "2017-06-17" };
            var context = new ExporterContextFromArgs(args);
            Assert.AreEqual(context.UpperBound, DateTime.Parse("2017-06-17").Date);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestUpperBoundError()
        {
            var args = new[] { "abc" };
            var context = new ExporterContextFromArgs(args);
            var date = context.UpperBound.ToString();
        }

        [TestMethod]
        public void TestWithoutUpperBound()
        {
            var args = new string[0];
            var context = new ExporterContextFromArgs(args);
            Assert.AreEqual(context.UpperBound, DateTime.Now.Date);
        }

        [TestMethod]
        public void TestUpperBoundWithOtherArgs()
        {
            var args = new[] { "2017-06-17", "blabla", "trallala" };
            var context = new ExporterContextFromArgs(args);
            Assert.AreEqual(context.UpperBound, DateTime.Parse("2017-06-17").Date);
        }

    }
}
