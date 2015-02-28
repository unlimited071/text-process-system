using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Server.Models;

namespace Server
{
    internal class Program
    {
        private static BufferBlock<string> _bufferWork;
        private static string _outputFile;

        private static void Main()
        {
            _outputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\results.txt");
            _bufferWork = new BufferBlock<string>();
            string baseAddress = Settings.BaseAddress;
            Task work = StarConsumingWork();
            UseCustomMadeServer(baseAddress);
            _bufferWork.Complete();
            work.Wait();
        }

        private static async Task StarConsumingWork()
        {
            while (await _bufferWork.OutputAvailableAsync().ConfigureAwait(false))
            {
                string work;
                if (_bufferWork.TryReceive(out work))
                {
                    IStatCountCalculation[] calculations =
                    {
                        new AlphanumericCountStatCalculation(), 
                        new NCountStatCalculation(), 
                        new ParagraphCountStatCalculation()
                    };
                    var calculator = new StatsCalculator(calculations);
                    Stat[] stats = calculator.Calculate(work);

                    var statsResult = new StringBuilder();
                    statsResult.AppendLine(work);
                    foreach (var stat in stats)
                        statsResult.AppendFormat("{0}: {1}\r\n", stat.Description, stat.Count);
                    statsResult.AppendLine("========================\r\n");

                    File.AppendAllText(_outputFile, statsResult.ToString());
                }
            }
        }

        private static void UseCustomMadeServer(string baseAddress)
        {
            using (var server = new HttpServer(baseAddress))
            {
                Ping(baseAddress);
                server.WorkRecieved += server_WorkRecieved;
                TellAndWait(baseAddress);
            }
        }

        private static void server_WorkRecieved(string work)
        {
            _bufferWork.SendAsync(work).Wait();
        }

        private static void TellAndWait(string baseAddress)
        {
            Console.Out.WriteLine("Listening on " + baseAddress);
            Console.Out.WriteLine("Press any key to shutdown server...");
            Console.ReadKey();
        }

        private static void Ping(string baseAddress)
        {
            Console.Out.Write("Ping...");
            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(baseAddress + "home/ping").Result;
            Console.Out.WriteLine(response.Content.ReadAsStringAsync().Result);
        }
    }
}