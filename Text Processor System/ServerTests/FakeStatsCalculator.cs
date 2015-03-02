using Server.Models;

namespace ServerTests
{
    public class FakeStatsCalculator : IStatsCalculator
    {
        public bool Executed { get; set; }

        public Stat[] Calculate(string input)
        {
            Executed = true;
            return new Stat[] {};
        }
    }
}