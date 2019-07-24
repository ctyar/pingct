using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tether
{
    internal class DnsTest : ITest
    {
        private readonly ConsoleManager _consoleManager;
        private readonly string _hostName;

        private bool _result;

        public DnsTest(ConsoleManager consoleManager, string hostName)
        {
            _consoleManager = consoleManager;
            _hostName = hostName;
        }

        public async Task<bool> Run()
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
            _consoleManager.Print("DNS: ", MessageType.Info);
            _consoleManager.PrintResult(_result, "OK", "Not working");
        }
    }
}