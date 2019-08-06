using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ctyar.Pingct
{
    internal class DnsTest : ITest
    {
        private readonly string _hostName;
        private readonly IReportManager _reportManager;

        private bool _result;

        public DnsTest(IReportManager reportManager, IOptions<Settings> settings)
        {
            _reportManager = reportManager;
            _hostName = settings.Value.Dns;
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
            var (message, type) = _result ? ("OK", MessageType.Success) : ("Not working", MessageType.Failure);

            _reportManager.Print("DNS: ", MessageType.Info);
            _reportManager.Print(message, type);
            _reportManager.PrintLine();
        }
    }
}