using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Server.Models
{
    public class TextStatsProcessor
    {
        private readonly IStatsCalculator _calculator;
        private readonly BufferBlock<string> _inputBuffer;
        private readonly IStatsPersister _persister;

        public TextStatsProcessor(IStatsCalculator statsCalculator, IStatsPersister persister)
        {
            _inputBuffer = new BufferBlock<string>();
            _persister = persister;
            _calculator = statsCalculator;
        }

        public async Task<bool> AddTextAsync(string text)
        {
            return await _inputBuffer.SendAsync(text);
        }

        public async Task StarAsync()
        {
            while (await _inputBuffer.OutputAvailableAsync().ConfigureAwait(false))
            {
                string input;
                if (_inputBuffer.TryReceive(out input))
                {
                    Stat[] stats = _calculator.Calculate(input);
                    _persister.Persist(input, stats);
                }
            }
        }

        public void Stop()
        {
            _inputBuffer.Complete();
        }
    }
}