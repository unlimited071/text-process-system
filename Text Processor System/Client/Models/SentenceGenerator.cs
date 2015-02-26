using System;
using System.Text;

namespace Client.Models
{
    public class SentenceGenerator : ISentenceGenerator
    {
        private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly string[] WordSeparators = {" ", " ", " ", ", ", ". "};
        private readonly Random _random = new Random();

        public string GenerateSentence()
        {
            var sentence = new StringBuilder();
            int wordCount = _random.Next(0, 10);
            for (int i = 0; i < wordCount; i++)
            {
                int wordSeparatorPos = _random.Next(0, WordSeparators.Length);
                sentence.Append(GenerateWord());
                sentence.Append(WordSeparators[wordSeparatorPos]);
            }
            sentence.Append(GenerateWord());
            return sentence + "." + Environment.NewLine;
        }

        public string GenerateWord()
        {
            var word = new StringBuilder();
            int wordLength = _random.Next(2, 10);
            for (int i = 0; i < wordLength; i++)
            {
                int charPos = _random.Next(0, Characters.Length);
                word.Append(Characters[charPos]);
            }
            return word.ToString();
        }
    }
}