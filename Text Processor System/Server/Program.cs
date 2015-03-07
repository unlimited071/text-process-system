using System;
using System.IO;
using System.Net.Http;
using Server.Models;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string outputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Settings.OutputFile);
            string baseAddress = Settings.BaseAddress;
            int numberOfWorkers;
            if (args.Length < 1 || !int.TryParse(args[0], out numberOfWorkers))
                numberOfWorkers = 200;
            UseCustomMadeServer(baseAddress, outputFile, numberOfWorkers);
        }

        private static void UseCustomMadeServer(string baseAddress, string outputFile, int numberOfWorkers)
        {
            var calculations = new ITextStatCalculation[]
            {
                new AlphanumericCountTextStatCalculation(),
                new NCountTextStatCalculation(),
                new ParagraphCountTextStatCalculation(),
                new SixteenOrMoreWordsSentenceTextStatCalculation()
            };
            IStatsCalculator statsCalculator = new StatsCalculator(calculations);
            IStatsPersister statsPersister = new StatsPersister(outputFile);
            var textStatsProcessor = new TextStatsProcessor(statsCalculator, statsPersister);
            var textStatsProcessorHandler = new TextStatsProcessorHandler("/", textStatsProcessor);

            IHttpListenerContextHandler[] handlers =
            {
                textStatsProcessorHandler,
                new PongHandler("/ping")
            };

            var options = new HttpServerOptions(baseAddress, handlers, numberOfWorkers);
            using (var server = new HttpServer(options))
            {
                server.Start();
                Ping(baseAddress);
                TellAndWait(baseAddress);
                server.Stop();
            }

            textStatsProcessorHandler.Complete();
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