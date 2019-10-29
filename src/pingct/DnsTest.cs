using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ctyar.Pingct
{
    internal class DnsTest : ITest
    {
        private readonly string _hostName;
        private readonly IConsoleManager _consoleManager;

        private bool _result;

        public DnsTest(IConsoleManager consoleManager, Settings settings)
        {
            _consoleManager = consoleManager;
            _hostName = settings.Dns;
            ServicePointManager.DnsRefreshTimeout = 0;
        }

        public async Task<bool> RunAsync()
        {
            _result = false;
            try
            {
                var ipAddresses = await Dns.GetHostAddressesAsync(_hostName);
                _result = ipAddresses.Any();
            }
            catch (SocketException)
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