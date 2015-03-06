using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Models
{
    public interface IStatsPersister
    {
        Task PersistAsync(string input, IEnumerable<Stat> stats);
    }
}