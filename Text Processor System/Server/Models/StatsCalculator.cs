using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Models
{
    public class StatsCalculator : IStatsCalculator
    {
        private readonly ITextStatCalculation[] _calculations;

        public StatsCalculator(ITextStatCalculation[] calculations)
        {
            _calculations = calculations;
        }

        public IEnumerable<Stat> Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            return _calculations.Select(c => c.Calculation(input));
        }
    }
}