using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using SocksSharp;
using SocksSharp.Proxy;

namespace Tether
{
    internal class TestManager
    {
        private const long MaxPingSuccessTime = 120;
        private const long MaxPingWarningTime = 170;
        private const int DelayTime = 1000;

        private static readonly ProxySettings ProxySettings = new ProxySettings
        {
            Host = "127.0.0.1",
            Port = 9150
        };
        private static readonly ProxyClientHandler<Socks5> ProxyClientHandler =
            new ProxyClientHandler<Socks5>(ProxySettings);
        private static readonly HttpClient HttpClient = new HttpClient(ProxyClientHandler);
        private readonly ConsoleManager _console;
        private static readonly string FreedomTestHost = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cHM6Ly9wb3JuaHViLmNvbQ=="));

        public TestManager() => _console = new ConsoleManager();

        public async Task Scan()
        {
            while (true)
            {
                await KeepPinging("4.2.2.4");

                while (true)
                {
                    await TestGateway("192.168.1.1");

                    await TestInCountryConnection("aparat.com");

                    await TestDns("www.facebook.com");

                    await TestFreedom(FreedomTestHost);

                    var areWeBackOnline = await AreWeBackOnline("4.2.2.4");

                    await Task.Delay(DelayTime);

                    if (areWeBackOnline)
                    {
                        break;
                    }
                }
            }
        }

        private async Task TestDns(string hostName)
        {
            bool succeed;
            try
            {
                var ipAddresses = await Dns.GetHostAddressesAsync(hostName);
                succeed = ipAddresses.Any();
            }
            catch
            {
                succeed = false;
            }

            _console.Print("DNS: ", MessageType.Info);
            _console.PrintResult(succeed, "OK", "Not working");
        }

        private async Task TestFreedom(string host)
        {
            bool succeed;
            try
            {
                var stream = await HttpClient.GetStreamAsync(host);
                succeed = true;
            }
            catch
            {
                succeed = false;
            }

            _console.Print("Freedom: ", MessageType.Info);
            _console.PrintResult(succeed, "OK", "Not working");
        }

        private async Task TestGateway(string host)
        {
            await Ping(host);
        }

        private async Task TestInCountryConnection(string host)
        {
            await Ping(host);
        }

        private async Task KeepPinging(string ip)
        {
            var ping = new Ping();
            
            while (true)
            {
                bool success;
                PingReply pingReply = default;
                try
                {
                    pingReply = await ping.SendPingAsync(ip);
                    success = pingReply.Status == IPStatus.Success;
                }
                catch
                {
                    success = false;
                }

                if (!success)
                {
                    return;
                }

                _console.PrintPing(ip, pingReply.RoundtripTime, MaxPingSuccessTime, MaxPingWarningTime);

                await Task.Delay(DelayTime);
            }
        }

        private async Task<bool> AreWeBackOnline(string ip)
        {
            var ping = new Ping();

            bool success;
            try
            {
                var pingReply = await ping.SendPingAsync(ip);
                success = pingReply.Status == IPStatus.Success;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        private async Task Ping(string host)
        {
            var ping = new Ping();

            long roundTripTime;
            try
            {
                roundTripTime = (await ping.SendPingAsync(host)).RoundtripTime;
            }
            catch
            {
                roundTripTime = 0;
            }

            _console.Print(string.Empty, MessageType.Info);
            _console.PrintPing(host, roundTripTime, MaxPingSuccessTime, MaxPingWarningTime);
        }
    }
}