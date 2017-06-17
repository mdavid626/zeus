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
    public class EnumModelBinderTest : ModelBinderTestBase
    {
        private enum TestEnum
        {
            Item1,
            Item2,
            Item3
        }

        [TestMethod]
        public void TestItem1()
        {
            var bindingContext = CreateBindingContext<TestEnum>("Item1");
            var binder = new EnumModelBinder<TestEnum>();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsTrue(result);
            Assert.AreEqual(bindingContext.Model, TestEnum.Item1);
        }

        [TestMethod]
        public void TestItem1Nullable()
        {
            var bindingContext = CreateBindingContext<TestEnum?>("Item1");
            var binder = new EnumModelBinder<TestEnum>();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsTrue(result);
            Assert.AreEqual(bindingContext.Model, TestEnum.Item1);
        }

        [TestMethod]
        public void TestItem1LowerCase()
        {
            var bindingContext = CreateBindingContext<TestEnum>("item2");
            var binder = new EnumModelBinder<TestEnum>();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsTrue(result);
            Assert.AreEqual(bindingContext.Model, TestEnum.Item2);
        }

        [TestMethod]
        public void TestItem1UpperCase()
        {
            var bindingContext = CreateBindingContext<TestEnum>("ITEM3");
            var binder = new EnumModelBinder<TestEnum>();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsTrue(result);
            Assert.AreEqual(bindingContext.Model, TestEnum.Item3);
        }

        [TestMethod]
        public void TestWrongItem()
        {
            var bindingContext = CreateBindingContext<TestEnum>("ItemX");
            var binder = new EnumModelBinder<TestEnum>();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsFalse(result);
            Assert.IsNull(bindingContext.Model);
        }

        [TestMethod]
        public void TestEmptyString()
        {
            var bindingContext = CreateBindingContext<TestEnum>("");
            var binder = new EnumModelBinder<TestEnum>();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsFalse(result);
            Assert.IsNull(bindingContext.Model);
        }

        [TestMethod]
        public void TestNullInput()
        {
            var bindingContext = CreateBindingContext<TestEnum>(null);
            var binder = new EnumModelBinder<TestEnum>();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsFalse(result);
            Assert.IsNull(bindingContext.Model);
        }
    }
}
