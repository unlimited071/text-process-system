using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server.Models
{
    public class StatsCalculator
    {
        private readonly IStatCountCalculation[] _calculations;

        public StatsCalculator(IStatCountCalculation[] calculations)
        {
            _calculations = calculations;
        }

        public Stat[] Calculate(string input)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (_calculations.Length == 0)
                return new Stat[0];

            var stats = new ConcurrentBag<Stat>();
            OrderablePartitioner<Tuple<int, int>> partitioner = Partitioner.Create(0, _calculations.Length);
            Parallel.ForEach(partitioner, (range, state) =>
            {
                if (state.ShouldExitCurrentIteration)
                    state.Break();

                for (int i = range.Item1; i < range.Item2; i++)
                    stats.Add(_calculations[i].Calculate(input));
            });
            return stats.ToArray();
        }
    }
}