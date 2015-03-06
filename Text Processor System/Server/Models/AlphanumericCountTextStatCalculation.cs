using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class AlphanumericCountTextStatCalculation : ITextStatCalculation
    {
        public Func<string, Stat> Calculation
        {
            get { return Calculate; }
        }

        private static Stat Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return new Stat
            {
                Description = "Alphanumeric Characters",
                Count = Regex.Matches(input, "[qwertyuiopasdfghjklzxcvbmQWERTYUIOPASDFGHJKLZXCVBM1234567890]").Count
            };
        }
    }
}