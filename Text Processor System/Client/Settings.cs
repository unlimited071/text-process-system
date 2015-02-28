using System;
using System.Configuration;

namespace Client
{
    internal static class Settings
    {
        public static Uri ServerUri
        {
            get
            {
                string serverUriString = ConfigurationManager.AppSettings["ServerUri"];
                return new Uri(serverUriString ?? "http://localhost:9000/home/post");
            }
        }
    }
}