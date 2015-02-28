using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class NCountStatCalculation : IStatCountCalculation
    {
        private readonly Regex _regex = new Regex("n[,. \r\n]");

        public Stat Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return new Stat
            {
                Description = "Words ending in n",
                Count = _regex.Matches(input).Count
            };
        }
    }
}