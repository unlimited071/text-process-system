﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string input = GetInput(args);
            while (input != "exit")
            {
                int number;
                if (int.TryParse(input, out number) == false)
                    Console.Out.WriteLine("Not a valid input, sending 0 requests");

                Console.Out.WriteLine("Sending...");

                Start(number);
                input = GetInput(null);
            }
            Console.Out.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void Start(int number)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Task execution = SendStrategy.ExecuteAsync(number);
            try
            {
                execution.Wait();
            }
            catch (AggregateException e)
            {
                foreach (var ex in e.Flatten().InnerExceptions)
                {
                    Console.Error.WriteLine(ex.Message);
                    for (Exception ie = ex.InnerException; ie != null; ie = ie.InnerException)
                        Console.Error.WriteLine(ie.Message);
                }
            }
            stopwatch.Stop();
            Console.Out.WriteLine("It took: " + stopwatch.ElapsedMilliseconds + "ms");
        }

        private static string GetInput(string[] args)
        {
            string input;
            if (args != null && args.Length > 0)
            {
                input = args[0];
            }
            else
            {
                Console.Out.Write("Please enter the number of texts to send: ");
                input = Console.ReadLine();
            }
            return input;
        }
    }
}