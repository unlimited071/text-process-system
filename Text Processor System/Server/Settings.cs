using System.Configuration;

namespace Server
{
    internal static class Settings
    {
        public static string BaseAddress
        {
            get
            {
                string baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
                return baseAddress ?? "http://localhost:9000/";
            }
        }

        public static string OutputFile
        {
            get
            {
                string outputFile = ConfigurationManager.AppSettings["OutputFile"];
                return outputFile ?? "..\\..\\..\\results.txt";
            }
        }
    }
}