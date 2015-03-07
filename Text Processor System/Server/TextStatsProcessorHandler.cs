using System.IO;
using System.Net;
using System.Threading.Tasks;
using Server.Models;

namespace Server
{
    internal class TextStatsProcessorHandler : HttpListenerContextHandler
    {
        private readonly ITextStatsProcessor _processor;

        public TextStatsProcessorHandler(string path, ITextStatsProcessor processor) : base(path)
        {
            _processor = processor;
        }

        public override async Task HandleAsync(HttpListenerContext context)
        {
            string input;
            using (var streamReader = new StreamReader(context.Request.InputStream))
                input = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            context.Response.StatusCode = (int) HttpStatusCode.OK;
            context.Response.Close();
            await _processor.AddTextAsync(input).ConfigureAwait(false);
        }

        public void Complete()
        {
            _processor.Complete();
        }
    }
}