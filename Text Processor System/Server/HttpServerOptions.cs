namespace Server
{
    public class HttpServerOptions
    {
        private readonly string _baseAddress;
        private readonly HttpHandlerAsync[] _handlersAsync;
        private readonly int _numberOfWorkers;

        public HttpServerOptions(string baseAddress, HttpHandlerAsync[] handlersAsync, int numberOfWorkers)
        {
            _baseAddress = baseAddress;
            _handlersAsync = handlersAsync;
            _numberOfWorkers = numberOfWorkers;
        }

        public string BaseAddress
        {
            get { return _baseAddress; }
        }

        public HttpHandlerAsync[] HandlersAsync
        {
            get { return _handlersAsync; }
        }

        public int NumberOfWorkers
        {
            get { return _numberOfWorkers; }
        }
    }
}