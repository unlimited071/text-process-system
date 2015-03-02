using System;
using System.Net;
using System.Threading.Tasks;

namespace Server
{
    public class HttpHandlerAsync
    {
        private readonly string _path;
        private readonly Func<HttpListenerContext, Task> _handlerBody;

        public HttpHandlerAsync(string path, Func<HttpListenerContext, Task> handlerBody)
        {
            _path = path;
            _handlerBody = handlerBody;
        }

        public bool CanHandlePath(string path)
        {
            return string.Compare(_path, path, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public async Task HandleAsync(HttpListenerContext context)
        {
            await _handlerBody(context);
        }
    }
}