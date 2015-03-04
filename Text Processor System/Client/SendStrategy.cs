using System.Net.Http;
using System.Threading.Tasks;
using Client.Models;

namespace Client
{
    public class SendStrategy
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static async Task ExecuteAsync(int number)
        {
            var tasks = new Task[number];
            var generator = new TextGenerator();

            for (int i = 0; i < number; i++)
                tasks[i] = SendTextsAsync(generator.GenerateText());

            await Task.WhenAll(tasks);
        }

        public static async Task SendTextsAsync(string text)
        {
            HttpResponseMessage response =
                await HttpClient.PostAsync(Settings.ServerUri, new StringContent(text));

            response.EnsureSuccessStatusCode();
        }
    }
}