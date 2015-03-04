using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Models
{
    public class StatsCalculator : IStatsCalculator
    {
        private readonly Func<string, Stat>[] _calculators;

        public StatsCalculator(Func<string, Stat>[] calculators)
        {
            _calculators = calculators;
        }

        public Stat[] Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");

            IEnumerable<Stat> stats =
                from calculation in _calculators.AsParallel()
                select calculation(input);

            return stats.ToArray();
        }
    }
}