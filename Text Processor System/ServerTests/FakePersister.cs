using Server.Models;

namespace ServerTests
{
    public class FakePersister : IStatsPersister
    {
        public bool Executed { get; set; }

        public void Persist(string input, Stat[] stats)
        {
            Executed = true;
        }
    }
}