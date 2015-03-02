using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class StatsPersisterAsync : IStatsPersisterAsync
    {
        private readonly string _outputFile;

        public StatsPersisterAsync(string outputFile)
        {
            _outputFile = outputFile;
        }

        public async Task PersistAsync(string input, Stat[] stats)
        {
            var statsResult = new StringBuilder();
            statsResult.AppendLine(input);
            foreach (Stat stat in stats)
                statsResult.AppendFormat("{0}: {1}\r\n", stat.Description, stat.Count);
            statsResult.AppendLine("========================\r\n");

            using (StreamWriter writer = File.AppendText(_outputFile))
                await writer.WriteAsync(statsResult.ToString());
        }
    }
}