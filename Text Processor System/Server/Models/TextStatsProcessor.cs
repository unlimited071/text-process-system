using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Server.Models
{
    public class TextStatsProcessor : ITextStatsProcessor
    {
        private readonly IStatsCalculator _calculator;
        private readonly ActionBlock<string> _inputBuffer;
        private readonly IStatsPersister _persister;

        public TextStatsProcessor(IStatsCalculator statsCalculator, IStatsPersister persister)
        {
            var options = new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = 1};
            _inputBuffer = new ActionBlock<string>((Func<string, Task>) ProcessAsync, options);
            _persister = persister;
            _calculator = statsCalculator;
        }

        public async Task<bool> AddTextAsync(string text)
        {
            return await _inputBuffer.SendAsync(text).ConfigureAwait(false);
        }

        private async Task ProcessAsync(string input)
        {
            IEnumerable<Stat> stats = _calculator.Calculate(input);
            await _persister.PersistAsync(input, stats);
        }

        public void Complete()
        {
            _inputBuffer.Complete();
            _inputBuffer.Completion.Wait();
        }
    }
}