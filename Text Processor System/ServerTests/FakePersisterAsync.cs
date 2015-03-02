using System.Threading.Tasks;
using Server.Models;

namespace ServerTests
{
    public class FakePersisterAsync : IStatsPersisterAsync
    {
        public bool Executed { get; set; }

        public void Persist(string input, Stat[] stats)
        {
            Executed = true;
        }

        public Task PersistAsync(string input, params Stat[] stats)
        {
            Executed = true;
            return Task.FromResult(0);
        }
    }
}