using System.Threading.Tasks;

namespace Server.Models
{
    public interface ITextStatsProcessor
    {
        Task<bool> AddTextAsync(string text);
        void Complete();
    }
}