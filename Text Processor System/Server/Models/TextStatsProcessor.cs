using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Server.Models
{
    public class TextStatsProcessor
    {
        private readonly IStatsCalculator _calculator;
        private readonly ActionBlock<string> _inputBuffer;
        private readonly IStatsPersisterAsync _persisterAsync;

        public TextStatsProcessor(IStatsCalculator statsCalculator, IStatsPersisterAsync persisterAsync)
        {
            var options = new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = 1};
            _inputBuffer = new ActionBlock<string>((Func<string, Task>) ProcessAsync, options);
            _persisterAsync = persisterAsync;
            _calculator = statsCalculator;
        }

        public async Task<bool> AddTextAsync(string text)
        {
            return await _inputBuffer.SendAsync(text);
        }

        public async Task ProcessAsync(string input)
        {
            Stat[] stats = _calculator.Calculate(input);
            await _persisterAsync.PersistAsync(input, stats);
        }

        public void Stop()
        {
            _inputBuffer.Complete();
            _inputBuffer.Completion.Wait();
        }
    }
}