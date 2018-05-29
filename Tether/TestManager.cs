using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Tether
{
    internal class TestManager : ITestManager
    {
        private static readonly WebClient WebClient = new WebClient();
        private readonly IReportManager _reportManager;

        public TestManager() => _reportManager = new ConsoleReportManager();

        public async Task Scan(Config config)
        {
            while (true)
            {
                await TestGateway("192.168.1.1");

                // TODO: Needs improvment
                TestProxy();

                await TestIsp("91.98.29.182");

                TestDns();

                await TestOverseaServer("4.2.2.4");

                Console.WriteLine();

                await Task.Delay(2000);
            }
        }

        private void TestDns()
        {
            var succeed = false;
            try
            {
                var ipHostEntry = Dns.GetHostEntry("www.google.com");
                succeed = ipHostEntry.AddressList.Length != 0;
            }
            catch
            {
                succeed = false;
            }

            Console.Write("DNS working: ");
            _reportManager.ReportResult(succeed);
        }

        private void TestProxy()
        {
            bool result;
            try
            {
                using (WebClient.OpenRead("http://clients3.google.com/generate_204"))
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            Console.Write("Proxy working: ");
            _reportManager.ReportResult(result);
        }

        private async Task TestGateway(string ip)
        {
            var ping = new Ping();

            var pingReply = await ping.SendPingAsync(ip);

            var success = pingReply.Status == IPStatus.Success;

            Console.Write("Reaching gateway: ");
            _reportManager.ReportResult(success);

            Console.Write("Gateway ping time: ");
            _reportManager.ReportValue(pingReply.RoundtripTime, 120, 170);
        }

        private async Task TestOverseaServer(string host)
        {
            var ping = new Ping();

            var pingReply = await ping.SendPingAsync(host);

            var success = pingReply.Status == IPStatus.Success;

            Console.Write("Reaching oversea server: ");
            _reportManager.ReportResult(success);

            Console.Write("Oversea server ping time: ");
            _reportManager.ReportValue(pingReply.RoundtripTime, 120, 170);
        }

        private async Task TestIsp(string ip)
        {
            var ping = new Ping();

            var pingReply = await ping.SendPingAsync(ip);

            var success = pingReply.Status == IPStatus.Success;

            Console.Write("Reaching ISP: ");
            _reportManager.ReportResult(success);

            Console.Write("ISP ping time: ");
            _reportManager.ReportValue(pingReply.RoundtripTime, 120, 170);
        }
    }
}