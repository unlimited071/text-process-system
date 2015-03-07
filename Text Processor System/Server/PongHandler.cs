using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class PongHandler : HttpListenerContextHandler
    {
        public PongHandler(string path) : base(path)
        {
        }

        public override async Task HandleAsync(HttpListenerContext context)
        {
            const string responseString = "Pong";
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentLength64 = responseBytes.Length;
            context.Response.ContentType = "text/plain";
            await context.Response.OutputStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            context.Response.Close();
        }
    }
}