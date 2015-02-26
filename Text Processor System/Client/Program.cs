using System;
using Client.Models;

namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var generator = new TextGenerator();
            for (var i = 0; i < 20; i++)
            {
                string text = generator.GenerateText();
                Console.Out.WriteLine(text);
            }
        }
    }
}