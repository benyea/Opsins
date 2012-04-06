using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Opsins.Tests
{
    /// <summary>
    /// InExtensions测试
    /// </summary>
    [TestClass]
    public class InExtensionsTest
    {
        [TestMethod]
        public void InStringTest()
        {
            string[] arr = {"ab", "cd", "ed", "ac"};
            string ts = "cd";
            bool expected = true;
            bool actual = ts.In(arr);

            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public void NotInIntTest()
        {
            int[] arr = {1, 2, 3, 4, 5, 6};
            int ts = 8;
            bool expected = false;
            bool actual = ts.In(arr);

            Assert.AreEqual(expected,actual);
        }
    }
}
