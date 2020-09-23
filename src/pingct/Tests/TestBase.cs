using System;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests
{
    internal abstract class TestBase : ITest
    {
        public abstract Task<bool> RunAsync();

        public abstract void Report(IConsoleManager consoleManager);

        protected async Task<TResult> ExecuteWithTimeoutAsync<TResult>(Func<Task<TResult>> action)
        {
            var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromMilliseconds(2500), TimeoutStrategy.Pessimistic);

            return await timeoutPolicy.ExecuteAsync(action);
        }
    }
}