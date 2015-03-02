using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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
            if (_calculators.Length == 0)
                return new Stat[0];

            var stats = new ConcurrentBag<Stat>();
            OrderablePartitioner<Tuple<int, int>> partitioner = Partitioner.Create(0, _calculators.Length);
            Parallel.ForEach(partitioner, (range, state) =>
            {
                if (state.ShouldExitCurrentIteration)
                    state.Break();

                for (int i = range.Item1; i < range.Item2; i++)
                    stats.Add(_calculators[i](input));
            });
            return stats.ToArray();
        }
    }
}