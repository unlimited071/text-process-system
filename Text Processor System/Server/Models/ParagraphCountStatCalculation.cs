using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class ParagraphCountStatCalculation : IStatCountCalculation
    {
        private readonly Regex _regex = new Regex("[.]\r?\n");

        public Stat Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return new Stat
            {
                Description = "Paragraphs",
                Count = _regex.Matches(input).Count
            };
        }
    }
}