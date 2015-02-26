using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Models;

namespace ServerTests
{
    [TestClass]
    public class StatsCalculatorTests
    {
        [TestMethod]
        public void MustCountNCharacterOcurrancesCorrectly()
        {
            const string input = "ae1fn derf, aeRFgn a98gn.\narjuN ansUn\n";
            const int expectedCount = 4;
            var calculation = new NCountStatCalculation();
            Stat result = calculation.Calculate(input);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MustCountParaghapsCorrectly()
        {
            const string input = "this is a sample paragraph.\r\nWith some text that\r\nDoesnt really matter.\n";
            const int expectedCount = 2;
            var calculation = new ParagraphCountStatCalculation();
            Stat result = calculation.Calculate(input);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MustCountAlphanumericCharsCorrectly()
        {
            const string input = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890,. \r\n";
            const int expectedCount = 60;
            var calculation = new AlphanumericCountStatCalculation();
            Stat result = calculation.Calculate(input);
            Assert.AreEqual(expectedCount, result.Count);
        }
    }
}