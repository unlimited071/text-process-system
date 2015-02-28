using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Client.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientTests
{
    [TestClass]
    public class SentenceGeneratorTests
    {
        private readonly ISentenceGenerator _sentenceGenerator = new SentenceGenerator();

        [TestMethod]
        public void ASenteceMustNotBeNullOrEmpty()
        {
            string sentence = _sentenceGenerator.GenerateSentence();
            Assert.IsFalse(string.IsNullOrEmpty(sentence));
        }

        [TestMethod]
        public void AllSentencesEndInPeriodAndNewLine()
        {
            string sentence = _sentenceGenerator.GenerateSentence();
            Assert.IsTrue(sentence.EndsWith("." + Environment.NewLine));
        }

        [TestMethod]
        public void SentencesAreGeneratedInLessThan1MilliSecond()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            _sentenceGenerator.GenerateSentence();
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1);
        }

        [TestMethod]
        public void SentecesAreMadeOfValidWords()
        {
            string sentence = _sentenceGenerator.GenerateSentence();
            var regex = new Regex("[^a-zA-Z0-9,. \r\n]");
            Assert.IsFalse(regex.IsMatch(sentence));
        }
    }
}