using System.Threading.Tasks;

namespace Server.Models
{
    public interface IStatsPersisterAsync
    {
        Task PersistAsync(string input, params Stat[] stats);
    }
}