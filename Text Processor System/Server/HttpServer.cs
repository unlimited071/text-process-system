using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class HttpServer : IDisposable
    {
        private readonly HttpHandlerAsync[] _handlersAsync;
        private readonly HttpListener _listener;
        private readonly int _numberOfWorkers;
        private bool _disposed;
        private Task _serverTask;

        private HttpServer(HttpServerOptions httpServerOptions)
        {
            _handlersAsync = httpServerOptions.HandlersAsync;
            _numberOfWorkers = httpServerOptions.NumberOfWorkers;
            _listener = new HttpListener();
            _listener.Prefixes.Add(httpServerOptions.BaseAddress);
            _listener.Start();
            StartWorkers();
        }

        public static HttpServer CreateHttpServer(HttpServerOptions httpServerOptions)
        {
            return new HttpServer(httpServerOptions);
        }

        private void StartWorkers()
        {
            var requestHandlers = new Task[_numberOfWorkers];
            for (int i = 0; i < _numberOfWorkers; i++)
                requestHandlers[i] = CreateRequestHandler();

            _serverTask = Task.WhenAll(requestHandlers);
        }

        private void Stop()
        {
            _listener.Stop();
            _serverTask.Wait();
        }

        private async Task CreateRequestHandler()
        {
            while (_listener.IsListening)
            {
                HttpListenerResponse response = null;
                try
                {
                    HttpListenerContext context = await _listener.GetContextAsync().ConfigureAwait(false);
                    response = context.Response;

                    if (! await TryHandleRequestAsync(context))
                    {
                        response.StatusCode = (int) HttpStatusCode.NotFound;
                        return;
                    }
                }
                catch (HttpListenerException)
                {
                    // It's ok, the listener has stopped
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
            }
        }

        private async Task<bool> TryHandleRequestAsync(HttpListenerContext context)
        {
            HttpHandlerAsync handlerAsync = _handlersAsync.FirstOrDefault(h => h.CanHandlePath(context.Request.RawUrl));
            if (handlerAsync == null) return false;

            Exception exception = null;
            try
            {
                await handlerAsync.HandleAsync(context);
            }
            catch (Exception e)
            {
                exception = e;
            }
            if (exception != null)
            {
                await HandleUnhandledException(context, exception);
            }
            return true;
        }

        private static async Task HandleUnhandledException(HttpListenerContext context, Exception exception)
        {
            HttpListenerResponse response = context.Response;
            byte[] responseBytes = Encoding.UTF8.GetBytes(exception.Message);
            response.ContentLength64 = responseBytes.Length;
            response.ContentType = "text/plain";
            await response.OutputStream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                Stop();

            if (_listener != null)
                _listener.Close();

            _disposed = true;
        }

        #endregion
    }
}