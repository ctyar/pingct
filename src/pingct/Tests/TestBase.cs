using System;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests
{
    internal abstract class TestBase : ITest
    {
        public async Task<bool> RunAsync()
        {
            var result = await RunCoreAsync();

            return result;
        }

        public void Report()
        {
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
