using System.Collections.Generic;
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
            var senders = new List<Task>();
            var generator = new TextGenerator();
            for (int i = 0; i < number; i++)
                senders.Add(SendTextsAsync(generator.GenerateText()));
            await Task.WhenAll(senders.ToArray()).ConfigureAwait(false);
        }

        public static async Task SendTextsAsync(string text)
        {
            HttpResponseMessage response =
                await HttpClient.PostAsync(Settings.ServerUri, new StringContent(text)).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
        }
    }
}