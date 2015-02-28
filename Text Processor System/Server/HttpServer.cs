using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class HttpServer : IDisposable
    {
        private const int NumberOfWorkers = 20;
        private readonly HttpListener _listener;
        private bool _disposed;
        private Task _serverTask;

        public HttpServer(string baseAddress)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(baseAddress);
            _listener.Start();
            Start();
        }

        public event Action<string> WorkRecieved;

        protected virtual void OnWorkRecieved(string work)
        {
            Action<string> handler = WorkRecieved;
            if (handler != null) handler(work);
        }

        private void Start()
        {
            var requestHandlers = new Task[NumberOfWorkers];
            for (int i = 0; i < NumberOfWorkers; i++)
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
                    HttpListenerRequest request = context.Request;
                    response = context.Response;

                    //Validate Requests
                    string httpMethod = request.HttpMethod;
                    if (httpMethod != "POST")
                    {
                        byte[] reponseContent = Encoding.UTF8.GetBytes("Only POST requests are allowed");
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        response.ContentType = "text/plain";
                        response.ContentLength64 = reponseContent.Length;
                        await
                            response.OutputStream.WriteAsync(reponseContent, 0, reponseContent.Length)
                                .ConfigureAwait(false);
                        return;
                    }

                    //Get request content
                    string content;
                    using (var reader = new StreamReader(request.InputStream))
                        content = await reader.ReadToEndAsync().ConfigureAwait(false);

                    //Process request content
                    OnWorkRecieved(content);

                    //Respond to client
                    response.ContentType = "text/plain";
                    response.StatusCode = (int) HttpStatusCode.OK;
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