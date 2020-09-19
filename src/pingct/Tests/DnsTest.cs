using System;
using System.Threading.Tasks;
using DnsClient;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests
{
    internal class DnsTest : TestBase
    {
        private readonly IConsoleManager _consoleManager;

        private readonly string _hostName;
        private bool _result;

        public DnsTest(IConsoleManager consoleManager, Settings settings)
        {
            _consoleManager = consoleManager;
            _hostName = settings.Dns;
        }

        public override async Task<bool> RunCoreAsync()
        {
            _result = false;

            try
            {
                var client = new LookupClient(new LookupClientOptions
                {
                    UseCache = false
                });

                var dnsQueryResponse = await ExecuteWithTimeoutAsync(
                    async () => await client.QueryAsync(_hostName, QueryType.A)
                );

                _result = !dnsQueryResponse.HasError;
            }
            catch (Exception e) when (e is DnsResponseException || e is TimeoutRejectedException ||
                                      e is ArgumentOutOfRangeException)
            {
            }

            return _result;
        }

        public override void ReportCore()
        {
            var (message, type) = _result ? ("OK", MessageType.Success) : ("Not working", MessageType.Failure);

            _consoleManager.Print("DNS: ", MessageType.Info);
            _consoleManager.Print(message, type);
            _consoleManager.PrintLine();
        }
    }
}