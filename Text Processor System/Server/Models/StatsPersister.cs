using System.IO;
using System.Text;

namespace Server.Models
{
    public class StatsPersister : IStatsPersister
    {
        private readonly string _outputFile;

        public StatsPersister(string outputFile)
        {
            _outputFile = outputFile;
        }

        public void Persist(string input, Stat[] stats)
        {
            var statsResult = new StringBuilder();
            statsResult.AppendLine(input);
            foreach (Stat stat in stats)
                statsResult.AppendFormat("{0}: {1}\r\n", stat.Description, stat.Count);
            statsResult.AppendLine("========================\r\n");

            File.AppendAllText(_outputFile, statsResult.ToString());
        }
    }
}