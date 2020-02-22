using System.Threading.Tasks;
using DnsClient;

namespace Ctyar.Pingct.Tests
{
    internal class DnsTest : ITest
    {
        private readonly IConsoleManager _consoleManager;

        private readonly string _hostName;
        private bool _result;

        public DnsTest(IConsoleManager consoleManager, Settings settings)
        {
            _consoleManager = consoleManager;
            _hostName = settings.Dns;
        }

        public async Task<bool> RunAsync()
        {
            _result = false;

            try
            {
                var client = new LookupClient {UseCache = false};

                var dnsQueryResponse = await client.QueryAsync(_hostName, QueryType.A);

                _result = !dnsQueryResponse.HasError;
            }
            catch (DnsResponseException)
            {
            }

            return _result;
        }

        public void Report()
        {
            var (message, type) = _result ? ("OK", MessageType.Success) : ("Not working", MessageType.Failure);

            _consoleManager.Print("DNS: ", MessageType.Info);
            _consoleManager.Print(message, type);
            _consoleManager.PrintLine();
        }
    }
}