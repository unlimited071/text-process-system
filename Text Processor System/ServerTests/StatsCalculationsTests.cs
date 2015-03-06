using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Models;

namespace ServerTests
{
    [TestClass]
    public class StatsCalculationsTests
    {
        [TestMethod]
        public void MustCountNCharacterOcurrancesCorrectly()
        {
            const string input = "ae1fn derf, aeRFgn a98gn.\narjuN ansUn\n";
            const int expectedCount = 4;
            Stat result = new NCountTextStatCalculation().Calculation(input);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MustCountParaghapsCorrectly()
        {
            const string input = "this is a sample paragraph.\r\nWith some text that\r\nDoesnt really matter.\n";
            const int expectedCount = 2;
            Stat result = new ParagraphCountTextStatCalculation().Calculation(input);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MustCountAlphanumericCharsCorrectly()
        {
            const string input = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890,. \r\n";
            const int expectedCount = 60;
            Stat result = new AlphanumericCountTextStatCalculation().Calculation(input);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MustCount16PlusWordSentencesCorrectly()
        {
            const string input =
                "word1 word2 word3 word4 word5, word6 word7.\r\nword1 word2 word3 word4 word5, word6 word7 word8 word9, word10 word11 word12 word13 word14 word15, word16. word1 word2 word3 word4 word5.\r\nword1 word2 word3 word4 word5, word6 word7.";
            const int expectedCount = 1;
            Stat result = new SixteenOrMoreWordsSentenceTextStatCalculation().Calculation(input);
            Assert.AreEqual(expectedCount, result.Count);
        }
    }
}