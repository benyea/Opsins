using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Opsins.Tests
{
    /// <summary>
    /// StringExtensionsTest 的摘要说明
    /// </summary>
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void IsEmptyTest()
        {
            string ts = null;
            bool excepted = true;
            bool actual = ts.IsEmpty();

            Assert.AreEqual(excepted,actual);
        }

        [TestMethod]
        public void IsNotEmptyTest()
        {
            string ts = "a";
            bool excepted = true;
            bool actual = ts.IsNotEmpty();

            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void IsWhiteSpaceTest()
        {
            string ts = "\t";
            bool excepted = true;
            bool actual = ts.IsWhiteSpace();

            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void IfEmptyTest()
        {
            string ts = null;
            string excepted = "abc";
            string actual = ts.IfEmpty("abc");

            Assert.AreEqual(excepted,actual);
        }

        [TestMethod]
        public void CompareTest()
        {
            string ts = "abc";
            string ts2 = "bcd";
            int excepted = -1;
            int actual = ts.Compare(ts2);

            Assert.AreEqual(excepted,actual);
        }

        [TestMethod]
        public void CompareIngoreTest()
        {
            string ts = "abc";
            string ts2 = "Abc";
            int excepted = 0;
            int actual = ts.Compare(ts2, true);

            Assert.AreEqual(excepted,actual);
        }

        [TestMethod]
        public void FormatWithTest()
        {
            string ts = "{0},{1}";
            string excepted = "ben,yea";
            string actual = ts.FormatWith("ben", "yea");

            Assert.AreEqual(excepted,actual);
        }
    }
}
