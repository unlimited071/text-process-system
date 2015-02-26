using System.Text.RegularExpressions;

namespace Server.Models
{
    public class NCountStatCalculation : IStatCountCalculation
    {
        private readonly Regex _regex = new Regex("n[,. \r\n]");

        public Stat Calculate(string input)
        {
            return new Stat
            {
                Count = _regex.Matches(input).Count
            };
        }
    }
}