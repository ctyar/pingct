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

    public DnsTest(Settings settings)
    {
        _hostName = settings.Dns;
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
                async (ct) => await client.QueryAsync(_hostName, QueryType.A, cancellationToken: ct),
                token
            );

            _result = !dnsQueryResponse.HasError;
        }
        catch (Exception e) when (e is DnsResponseException || e is TimeoutRejectedException ||
                                  e is ArgumentOutOfRangeException)
        {
        }

        return _result;
    }

    public override void Report(PanelManager panelManager)
    {
        var (message, type) = _result ? (_hostName, MessageType.Success) : (_hostName, MessageType.Failure);

        panelManager.Print("DNS: ", MessageType.Info);
        panelManager.Print(message, type);
        panelManager.PrintLine();
    }
}