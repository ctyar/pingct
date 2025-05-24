using System.Threading;
using System.Threading.Tasks;

namespace Ctyar.Pingct.Tests;

internal interface ITest
{
    string Name { get; }

    Task<bool> RunAsync(CancellationToken cancellationToken);

    void Report(PanelManager panelManager);
}