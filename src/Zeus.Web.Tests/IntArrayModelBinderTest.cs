using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zeus.Web.Binders;

namespace Zeus.Web.Tests
{
    [TestClass]
    public class IntArrayModelBinderTest : ModelBinderTestBase
    {
        [TestMethod]
        public void TestSimpleArray()
        {
            var bindingContext = CreateBindingContext<int[]>("1,2,3");
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsTrue(result);
            Assert.IsTrue(bindingContext.ModelState.IsValid);
            Assert.IsTrue(new [] { 1, 2, 3 }.SequenceEqual((int[])bindingContext.Model));
        }

        [TestMethod]
        public void TestOneItemArray()
        {
            var bindingContext = CreateBindingContext<int[]>("1");
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsTrue(result);
            Assert.IsTrue(bindingContext.ModelState.IsValid);
            Assert.IsTrue(new[] { 1 }.SequenceEqual((int[])bindingContext.Model));
        }

        [TestMethod]
        public void TestEmptyArray()
        {
            var bindingContext = CreateBindingContext<int[]>("");
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsTrue(result);
            Assert.IsTrue(bindingContext.ModelState.IsValid);
            Assert.IsTrue(new int[0].SequenceEqual((int[])bindingContext.Model));
        }

        [TestMethod]
        public void TestNullArray()
        {
            var bindingContext = CreateBindingContext<int[]>(null);
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsFalse(result);
            Assert.IsTrue(bindingContext.ModelState.IsValid);
            Assert.IsNull(bindingContext.Model);
        }

        [TestMethod]
        public void TestWrongDelimiter()
        {
            var bindingContext = CreateBindingContext<int[]>("1;2;3");
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsFalse(result);
            Assert.IsFalse(bindingContext.ModelState.IsValid);
            Assert.IsNull(bindingContext.Model);
        }

        [TestMethod]
        public void TestDelimiterAtEnd()
        {
            var bindingContext = CreateBindingContext<int[]>("1,2,3,");
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsTrue(result);
            Assert.IsTrue(bindingContext.ModelState.IsValid);
            Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual((int[])bindingContext.Model));
        }

        [TestMethod]
        public void TestNotNumber()
        {
            var bindingContext = CreateBindingContext<int[]>("abc");
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsFalse(result);
            Assert.IsFalse(bindingContext.ModelState.IsValid);
            Assert.IsNull(bindingContext.Model);
        }

        [TestMethod]
        public void TestNotNumberInArrayStart()
        {
            var bindingContext = CreateBindingContext<int[]>("a,2,3");
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsFalse(result);
            Assert.IsFalse(bindingContext.ModelState.IsValid);
            Assert.IsNull(bindingContext.Model);
        }

        [TestMethod]
        public void TestNotNumberInArrayEnd()
        {
            var bindingContext = CreateBindingContext<int[]>("1,2,a");
            var binder = new IntArrayModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsFalse(result);
            Assert.IsFalse(bindingContext.ModelState.IsValid);
            Assert.IsNull(bindingContext.Model);
        }
    }
}
