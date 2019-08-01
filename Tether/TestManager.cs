using System.Linq;
using System.Threading.Tasks;

namespace Tether
{
    internal class TestManager
    {
        private const int DelayTime = 1000;
        private readonly IReportManager _reportManager;

        public TestManager()
        {
            _reportManager = new ReportManager();
        }

        public async Task Scan()
        {
            ITest[] tests =
            {
                new GatewayTest(_reportManager, "192.168.1.1"),
                new InCountryConnectionTest(_reportManager, "aparat.com"),
                new DnsTest(_reportManager, "www.facebook.com"),
                new FreedomTest(_reportManager)
            };

            while (true)
            {
                await KeepPinging("4.2.2.4");

                while (true)
                {
                    await RunAllTests(tests);

                    PrintAllTests(tests);

                    var areWeBackOnline = await AreWeBackOnline("4.2.2.4");
                    
                    if (areWeBackOnline)
                    {
                        break;
                    }
                }
            }
        }

        private async Task RunAllTests(ITest[] tests)
        {
            var tasks = tests.Select(item => item.Run()).ToList();

            await Task.WhenAll(tasks);
        }

        private void PrintAllTests(ITest[] tests)
        {
            foreach (var test in tests)
            {
                _reportManager.Print("    ", MessageType.Info);
                test.Report();
            }
        }

        private async Task KeepPinging(string hostName)
        {
            var pingTest = new PingTest(_reportManager, hostName, PingReportType.JustValue);

            while (true)
            {
                var isStillOnline = await pingTest.Run();

                if (!isStillOnline)
                {
                    return;
                }

                pingTest.Report();

                await Task.Delay(DelayTime);
            }
        }

        private Task<bool> AreWeBackOnline(string hostName)
        {
            var pingTest = new PingTest(_reportManager, hostName, PingReportType.NoReport);

            return pingTest.Run();
        }
    }
}