using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Server
{
    public class HttpServer : IDisposable
    {
        private readonly ActionBlock<HttpListenerContext> _buffer; 
        private readonly HttpHandlerAsync[] _handlersAsync;
        private readonly HttpListener _listener;
        private readonly int _numberOfWorkers;
        private bool _disposed;
        private Task _serverTask;

        private HttpServer(HttpServerOptions httpServerOptions)
        {
            _handlersAsync = httpServerOptions.HandlersAsync;
            _numberOfWorkers = httpServerOptions.NumberOfWorkers;
            _buffer = new ActionBlock<HttpListenerContext>((Func<HttpListenerContext, Task>) HandleRequestAsync);
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

            Task workerTasks = Task.WhenAll(requestHandlers);
            Task handlerTasks = _buffer.Completion;
            _serverTask = Task.WhenAll(workerTasks, handlerTasks);
        }

        private void Stop()
        {
            _listener.Stop();
            _buffer.Complete();
            try
            {
                _serverTask.Wait();
            }
            catch (AggregateException e)
            {
                foreach (var ex in e.Flatten().InnerExceptions)
                {
                    Console.Out.WriteLine(ex.Message);
                    for (Exception ie = ex.InnerException; ie != null; ie = ie.InnerException)
                        Console.Out.WriteLine(ie.Message);
                }
            }
        }

        private async Task CreateRequestHandler()
        {
            while (_listener.IsListening)
            {
                try
                {
                    HttpListenerContext context = await _listener.GetContextAsync().ConfigureAwait(false);
                    await _buffer.SendAsync(context);
                }
                catch (HttpListenerException)
                {
                    // It's ok, the listener has stopped
                }
            }
        }

        private async Task HandleRequestAsync(HttpListenerContext context)
        {
            HttpHandlerAsync handlerAsync = _handlersAsync.FirstOrDefault(h => h.CanHandlePath(context.Request.RawUrl));
            if (handlerAsync == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.Close();
                return;
            }

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
        }

        private static async Task HandleUnhandledException(HttpListenerContext context, Exception exception)
        {
            HttpListenerResponse response = context.Response;
            byte[] responseBytes = Encoding.UTF8.GetBytes(exception.Message);
            response.ContentLength64 = responseBytes.Length;
            response.ContentType = "text/plain";
            await response.OutputStream.WriteAsync(responseBytes, 0, responseBytes.Length);
            response.Close();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
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