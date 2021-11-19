using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests
{
    internal abstract class TestBase : ITest
    {
        public abstract Task<bool> RunAsync(CancellationToken cancellationToken);

        public abstract void Report(PanelManager panelManager);

        protected static async Task<TResult> ExecuteWithTimeoutAsync<TResult>(Func<CancellationToken, Task<TResult>> action,
            CancellationToken cancellationToken)
        {
            var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromMilliseconds(2500), TimeoutStrategy.Pessimistic);

            return await timeoutPolicy.ExecuteAsync(action, cancellationToken);
        }
    }
}