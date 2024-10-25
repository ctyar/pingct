using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests;

internal class HttpGetTest : TestBase
{
    private static readonly HttpClient HttpClient = new();
    private readonly string _hostName;
    private bool _result;

    public override string Name => "Get";

    public HttpGetTest(string hostName)
    {
        _hostName = hostName;
    }

    public override async Task<bool> RunAsync(CancellationToken cancellationToken)
    {
        _result = false;

        try
        {
            var _ = await ExecuteWithTimeoutAsync(
                async _ => await HttpClient.GetStreamAsync(_hostName),
                cancellationToken
            );

            _result = true;
        }
        catch (Exception e) when (e is HttpRequestException or OperationCanceledException or IOException or
            TimeoutRejectedException)
        {
        }

        return _result;
    }

    public override void Report(PanelManager panelManager)
    {
        var messageType = _result ? MessageType.Success : MessageType.Failure;

        panelManager.Print("GET: ", MessageType.Info);
        panelManager.Print(_hostName, messageType);
        panelManager.PrintLine();
    }
}
