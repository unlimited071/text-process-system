using System;
using System.Text.RegularExpressions;

namespace Server.Models
{
    public class ParagraphCountTextStatCalculation : ITextStatCalculation
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
                Description = "Paragraphs",
                Count = Regex.Matches(input, "[.]\r?\n").Count
            };
        }
    }
}