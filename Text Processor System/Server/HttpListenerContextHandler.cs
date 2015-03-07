using System;
using System.Net;
using System.Threading.Tasks;

namespace Server
{
    public abstract class HttpListenerContextHandler : IHttpListenerContextHandler
    {
        private readonly string _path;

        protected HttpListenerContextHandler(string path)
        {
            _path = path;
        }

        public virtual bool CanHandlePath(string path)
        {
            return string.Compare(_path, path, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public abstract Task HandleAsync(HttpListenerContext context);
    }
}