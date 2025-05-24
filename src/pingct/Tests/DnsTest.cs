using System;
using System.Threading;
using System.Threading.Tasks;
using DnsClient;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests;

internal class DnsTest : TestBase
{
    private readonly string _hostName;
    private bool _result;

    public override string Name => "Dns";

    public DnsTest(string hostName)
    {
        _hostName = hostName;
    }

    public override async Task<bool> RunAsync(CancellationToken token)
    {
        _result = false;

        try
        {
            var client = new LookupClient(new LookupClientOptions
            {
                UseCache = false
            });

            var dnsQueryResponse = await ExecuteWithTimeoutAsync(
                async ct => await client.QueryAsync(_hostName, QueryType.A, cancellationToken: ct),
                token
            );

            _result = !dnsQueryResponse.HasError;
        }
        catch (Exception e) when (e is DnsResponseException or TimeoutRejectedException or ArgumentOutOfRangeException)
        {
        }

        return _result;
    }

    public override void Report(PanelManager panelManager)
    {
        var messageType = _result ? MessageType.Success : MessageType.Failure;

        panelManager.Print("DNS: ", MessageType.Info);
        panelManager.Print(_hostName, messageType);
        panelManager.PrintLine();
    }
}