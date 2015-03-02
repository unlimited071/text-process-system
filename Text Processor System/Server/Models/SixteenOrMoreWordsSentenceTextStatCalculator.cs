using System.Text.RegularExpressions;

namespace Server.Models
{
    public class SixteenOrMoreWordsSentenceTextStatCalculator : ITextStatCalculator
    {
        private static readonly Regex Regex = new Regex("([a-zA-Z0-9]+[, ]+){15,}[a-zA-Z0-9]+[.][^\r\n]");

        Stat ITextStatCalculator.Calculate(string input)
        {
            return Calculate(input);
        }

        public static Stat Calculate(string input)
        {
            return new Stat
            {
                Description = "16+ word sentences",
                Count = Regex.Matches(input).Count
            };
        }
    }
}