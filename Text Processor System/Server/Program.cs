using System;
using Microsoft.Owin.Hosting;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string baseAddress = Settings.BaseAddress;

            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.Out.WriteLine("Press any key to shutdown server...");
                Console.ReadKey();
            }
        }
    }
}