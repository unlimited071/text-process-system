using System.Diagnostics;
using System.Text;
using Client.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClientTests
{
    [TestClass]
    public class TextGeneratorTests
    {
        private readonly TextGenerator _textGenerator = new TextGenerator();

        [TestMethod]
        public void TextMustNotBeNullOrEmpty()
        {
            string text = _textGenerator.GenerateText();
            Assert.IsFalse(string.IsNullOrEmpty(text));
        }

        [TestMethod]
        public void TextMustBeAtLeast1KbInSize()
        {
            const int expectedByteCount = 1000;
            string text = _textGenerator.GenerateText();
            int byteCount = Encoding.UTF8.GetByteCount(text);
            Assert.IsTrue(byteCount >= expectedByteCount);
        }

        [TestMethod]
        public void TextIsGeneratedInLessThan1MilliSecond()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            _textGenerator.GenerateText();
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1);
        }

        [TestMethod]
        public void Generating1KTextsShouldBeFast()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
                _textGenerator.GenerateText();
            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < 100);
        }
    }
}