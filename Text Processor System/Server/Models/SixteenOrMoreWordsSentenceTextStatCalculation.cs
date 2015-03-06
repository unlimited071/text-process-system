using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class SixteenOrMoreWordsSentenceTextStatCalculation : ITextStatCalculation
    {
        public Func<string, Stat> Calculation
        {
            get { return Calculate; }
        }

        private static Stat Calculate(string input)
        {
            return new Stat
            {
                Description = "16+ word sentences",
                Count = Regex.Matches(input, "([a-zA-Z0-9]+[, ]+){15,}[a-zA-Z0-9]+[.][^\r\n]").Count
            };
        }
    }
}