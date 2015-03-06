using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class NCountTextStatCalculation : ITextStatCalculation
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
                Description = "Words ending in n",
                Count = Regex.Matches(input, "n[,. \r\n]").Count
            };
        }
    }
}