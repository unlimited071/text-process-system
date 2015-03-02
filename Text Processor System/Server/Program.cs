using System;
using System.IO;
using System.Net.Http;
using Server.Models;

namespace Server
{
    internal class Program
    {
        private static void Main()
        {
            string outputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Settings.OutputFile);
            string baseAddress = Settings.BaseAddress;
            UseCustomMadeServer(baseAddress, outputFile);
        }

        private static void UseCustomMadeServer(string baseAddress, string outputFile)
        {
            var calculations = new Func<string, Stat>[]
            {
                AlphanumericCountTextStatCalculator.Calculate,
                NCountTextStatCalculator.Calculate,
                ParagraphCountTextStatCalculator.Calculate,
                SixteenOrMoreWordsSentenceTextStatCalculator.Calculate
            };
            IStatsCalculator statsCalculator = new StatsCalculator(calculations);
            IStatsPersisterAsync statsPersisterAsync = new StatsPersisterAsync(outputFile);
            var textStatsProcessor = new TextStatsProcessor(statsCalculator, statsPersisterAsync);
            var textProcessorHandler = new TextProcessorHandler(textStatsProcessor);
            textProcessorHandler.Start();

            var handlers = new[]
            {
                new HttpHandlerAsync("/", textProcessorHandler.HandleAsync),
                new HttpHandlerAsync("/ping", PongHandler.HandleAsync)
            };

            var options = new HttpServerOptions(baseAddress, handlers, 20000);
            using (HttpServer.CreateHttpServer(options))
            {
                Ping(baseAddress);
                TellAndWait(baseAddress);
            }

            textProcessorHandler.Stop();
        }

        private static void TellAndWait(string baseAddress)
        {
            Console.Out.WriteLine("Listening on " + baseAddress);
            Console.Out.WriteLine("Press any key to shutdown server...");
            Console.ReadKey();
            Console.Out.WriteLine("Shutting down, please wait while pending work completes...");
        }

        private static void Ping(string baseAddress)
        {
            Console.Out.Write("Ping...");
            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(baseAddress + "ping").Result;
            Console.Out.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}