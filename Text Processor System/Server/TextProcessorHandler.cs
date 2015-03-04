using System.Net;
using System.Threading.Tasks;
using Server.Models;

namespace Server
{
    internal class TextProcessorHandler
    {
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

        public void Stop()
        {
            _processor.Stop();
        }
    }
}