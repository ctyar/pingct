using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests
{
    internal abstract class TestBase : ITest
    {
        private readonly IConsoleManager _consoleManager;

        private long _time;

        protected TestBase(IConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
        }

        public async Task<bool> RunAsync()
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif            
            var result = await RunCoreAsync();
#if DEBUG
            stopwatch.Stop();
            _time = stopwatch.ElapsedMilliseconds;
#endif
            return result;
        }

        public void Report()
        {
#if DEBUG
            _consoleManager.Print($"({_time})");
#endif
            ReportCore();
        }

        protected async Task<TResult> ExecuteWithTimeoutAsync<TResult>(Func<Task<TResult>> action)
        {
            var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromMilliseconds(2500), TimeoutStrategy.Pessimistic);

            return await timeoutPolicy.ExecuteAsync(action);
        }

        public abstract Task<bool> RunCoreAsync();

        public abstract void ReportCore();
    }
}
