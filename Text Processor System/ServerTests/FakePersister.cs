using System.Collections.Generic;
using System.Threading.Tasks;
using Server.Models;

namespace ServerTests
{
    public class FakePersister : IStatsPersister
    {
        public bool Executed { get; private set; }

        public Task PersistAsync(string input, IEnumerable<Stat> stats)
        {
            Executed = true;
            return Task.FromResult(0);
        }
    }
}