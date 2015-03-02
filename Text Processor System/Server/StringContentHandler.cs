using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Server
{
    internal class StringContentHandler
    {
        public static async Task<string> HandleAsync(HttpListenerContext context)
        {
            string input;
            using (var streamReader = new StreamReader(context.Request.InputStream))
                input = await streamReader.ReadToEndAsync();
            return input;
        }
    }
}