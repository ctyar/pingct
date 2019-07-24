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

        public DnsTest(ConsoleManager consoleManager, string hostName)
        {
            _consoleManager = consoleManager;
            _hostName = hostName;
        }

        public async Task<bool> Run()
        {
            var result = false;
            try
            {
                var ipAddresses = await Dns.GetHostAddressesAsync(_hostName);
                result = ipAddresses.Any();
            }
            catch (SocketException)
            {
            }

            _consoleManager.Print("DNS: ", MessageType.Info);
            _consoleManager.PrintResult(result, "OK", "Not working");

            return result;
        }
    }
}