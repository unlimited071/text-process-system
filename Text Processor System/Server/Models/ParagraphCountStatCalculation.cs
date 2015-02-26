using System.Text.RegularExpressions;

namespace Server.Models
{
    public class ParagraphCountStatCalculation : IStatCountCalculation
    {
        private readonly Regex _regex = new Regex("[.]\r?\n");

        public Stat Calculate(string input)
        {
            return new Stat
            {
                Count = _regex.Matches(input).Count
            };
        }
    }
}