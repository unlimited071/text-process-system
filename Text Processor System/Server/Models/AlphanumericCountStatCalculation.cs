using System.Text.RegularExpressions;

namespace Server.Models
{
    public class AlphanumericCountStatCalculation : IStatCountCalculation
    {
        private readonly Regex _regex = new Regex("[qwertyuiopasdfghjklzxcvbmQWERTYUIOPASDFGHJKLZXCVBM1234567890]");

        public Stat Calculate(string input)
        {
            return new Stat
            {
                Count = _regex.Matches(input).Count
            };
        }
    }
}