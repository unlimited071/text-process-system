using System.Net.Http;
using System.Threading.Tasks;
using Client.Models;

namespace Client
{
    public class TextSender : ITextSender
    {
        private readonly ITextGenerator _generator;
        private readonly HttpClient _httpClient;

        public TextSender()
        {
            _httpClient = new HttpClient();
            _generator = new TextGenerator();
        }

        private async Task SendTextsAsync(string text)
        {
            HttpResponseMessage response =
                await _httpClient.PostAsync(Settings.ServerUri, new StringContent(text));

            response.EnsureSuccessStatusCode();
        }

        public void Send(int number)
        {
            var tasks = new Task[number];
            for (int i = 0; i < number; i++)
            {
                tasks[i] = SendTextsAsync(_generator.GenerateText());
            }
            Task.WaitAll(tasks);
        }
    }
}