using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using serverChat;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SM_1()
        {
            string str = "sqmes";
            int expected = 1;

            Program p = new Program();
            int actual = p.SM(str);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SM_2()
        {
            string str = "sqmes_1";
            int expected = 0;

            Program p = new Program();
            int actual = p.SM(str);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SM_3()
        {
            string str = "sqgmes";
            int expected = 0;

            Program p = new Program();
            int actual = p.SM(str);

            Assert.AreEqual(expected, actual);
        }
    }
}
