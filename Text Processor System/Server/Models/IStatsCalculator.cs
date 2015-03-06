using System.Collections.Generic;

namespace Server.Models
{
    public interface IStatsCalculator
    {
        IEnumerable<Stat> Calculate(string input);
    }
}