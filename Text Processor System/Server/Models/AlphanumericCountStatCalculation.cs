using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class AlphanumericCountStatCalculation : IStatCountCalculation
    {
        private readonly Regex _regex = new Regex("[qwertyuiopasdfghjklzxcvbmQWERTYUIOPASDFGHJKLZXCVBM1234567890]");

        public Stat Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return new Stat
            {
                Description = "Alphanumeric Characters",
                Count = _regex.Matches(input).Count
            };
        }
    }
}