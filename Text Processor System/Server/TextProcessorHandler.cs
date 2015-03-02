using System.Net;
using System.Threading.Tasks;

namespace Server
{
    internal class TextProcessorHandler
    {
        private static Task _work;
        private readonly TextStatsProcessor _processor;

        public TextProcessorHandler(TextStatsProcessor processor)
        {
            _processor = processor;
        }

        public async Task HandleAsync(HttpListenerContext context)
        {
            string input = await StringContentHandler.HandleAsync(context);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.Close();
            await _processor.AddTextAsync(input);
        }

        public void Start()
        {
            _work = _processor.StarAsync();
        }

        public void Stop()
        {
            _processor.Stop();
            _work.Wait();
        }
    }
}