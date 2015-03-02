using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Server.Models
{
    public class TextStatsProcessor
    {
        private readonly IStatsCalculator _calculator;
        private readonly BufferBlock<string> _inputBuffer;
        private readonly IStatsPersisterAsync _persisterAsync;

        public TextStatsProcessor(IStatsCalculator statsCalculator, IStatsPersisterAsync persisterAsync)
        {
            _inputBuffer = new BufferBlock<string>();
            _persisterAsync = persisterAsync;
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
                    await _persisterAsync.PersistAsync(input, stats).ConfigureAwait(false);
                }
            }
        }

        public void Stop()
        {
            _inputBuffer.Complete();
        }
    }
}