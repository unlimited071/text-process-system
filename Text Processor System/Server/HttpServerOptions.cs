namespace Server
{
    public class HttpServerOptions
    {
        private readonly string _baseAddress;
        private readonly IHttpListenerContextHandler[] _httpListenerContextHandlers;
        private readonly int _numberOfWorkers;

        public HttpServerOptions(string baseAddress, IHttpListenerContextHandler[] httpListenerContextHandlers, int numberOfWorkers)
        {
            _baseAddress = baseAddress;
            _httpListenerContextHandlers = httpListenerContextHandlers;
            _numberOfWorkers = numberOfWorkers;
        }

        public string BaseAddress
        {
            get { return _baseAddress; }
        }

        public IHttpListenerContextHandler[] HttpListenerContextHandlers
        {
            get { return _httpListenerContextHandlers; }
        }

        public int NumberOfWorkers
        {
            get { return _numberOfWorkers; }
        }
    }
}