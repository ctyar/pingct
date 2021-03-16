using System.Threading;
using System.Threading.Tasks;

namespace Ctyar.Pingct.Tests
{
    internal interface ITest
    {
        Task<bool> RunAsync(CancellationToken cancellationToken);

        void Report(PanelManager panelManager);
    }
}