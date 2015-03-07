using System.Net;
using System.Threading.Tasks;

namespace Server
{
    public interface IHttpListenerContextHandler
    {
        bool CanHandlePath(string path);
        Task HandleAsync(HttpListenerContext context);
    }
}