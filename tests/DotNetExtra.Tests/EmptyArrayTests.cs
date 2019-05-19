using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtra.Tests {

    [TestClass]
    public class EmptyArrayTests {

        [TestMethod]
        public void Value_IsEmpty() {
            CollectionAssert.AreEqual(new int[0], EmptyArray.Value<int>());
            CollectionAssert.AreEqual(new string[0], EmptyArray.Value<string>());
            CollectionAssert.AreEqual(new Uri[0], EmptyArray.Value<Uri>());
            CollectionAssert.AreEqual(new Foo[0], EmptyArray.Value<Foo>());
        }

        [TestMethod]
        public void Value_IsSingleton() {
            Assert.AreSame(EmptyArray.Value<int>(), EmptyArray.Value<int>());
            Assert.AreSame(EmptyArray.Value<string>(), EmptyArray.Value<string>());
            Assert.AreSame(EmptyArray.Value<Uri>(), EmptyArray.Value<Uri>());
            Assert.AreSame(EmptyArray.Value<Foo>(), EmptyArray.Value<Foo>());
        }

        #region Helpers

        private class Foo { }

        #endregion Helpers
    }
}