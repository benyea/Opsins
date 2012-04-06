using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Opsins.Tests
{
    /// <summary>
    /// ObjectExtensionsTest 的摘要说明
    /// </summary>
    [TestClass]
    public class ObjectExtensionsTest
    {
        /// <summary>
        /// 判断是否为NULL
        /// </summary>
        [TestMethod]
        public void IsNullTest()
        {
            object o = "123";
            bool excepted = false;
            bool actual = o.IsNull();

            Assert.AreEqual(excepted,actual);
        }

        /// <summary>
        /// 判断不为NULL
        /// </summary>
        [TestMethod]
        public void IsNotNullTest()
        {
            object o = "00";
            bool excepted = true;
            bool actual = o.IsNotNull();

            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void IfNullTest()
        {
            string o = null;
            string excepted = "abc";
            string actual = o.IfNull(GetAbc);

            Assert.AreEqual(excepted, actual);
        }

        private string GetAbc()
        {
            return "abc";
        }
    }
}
