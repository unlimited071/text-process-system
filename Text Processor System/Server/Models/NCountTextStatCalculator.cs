using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class NCountTextStatCalculator : ITextStatCalculator
    {
        private static readonly Regex Regex = new Regex("n[,. \r\n]");

        public static Stat Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return new Stat
            {
                Description = "Words ending in n",
                Count = Regex.Matches(input).Count
            };
        }

        Stat ITextStatCalculator.Calculate(string input)
        {
            return Calculate(input);
        }
    }
}