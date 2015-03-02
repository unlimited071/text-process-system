using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class AlphanumericCountTextStatCalculator : ITextStatCalculator
    {
        private static readonly Regex Regex = new Regex("[qwertyuiopasdfghjklzxcvbmQWERTYUIOPASDFGHJKLZXCVBM1234567890]");

        public static Stat Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return new Stat
            {
                Description = "Alphanumeric Characters",
                Count = Regex.Matches(input).Count
            };
        }

        Stat ITextStatCalculator.Calculate(string input)
        {
            return Calculate(input);
        }
    }
}