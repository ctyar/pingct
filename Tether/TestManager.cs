using System.Threading.Tasks;

namespace Tether
{
    internal class TestManager
    {
        private const int DelayTime = 1000;
        private readonly ConsoleManager _consoleManager;

        public TestManager()
        {
            _consoleManager = new ConsoleManager();
        }

        public async Task Scan()
        {
            ITest[] tests =
            {
                new GatewayTest(_consoleManager, "192.168.1.1"),
                new InCountryConnectionTest(_consoleManager, "aparat.com"),
                new DnsTest(_consoleManager, "www.facebook.com"),
                new FreedomTest(_consoleManager)
            };

            while (true)
            {
                await KeepPinging("4.2.2.4");

                while (true)
                {
                    foreach (var test in tests)
                    {
                        await test.Run();
                    }

                    var areWeBackOnline = await AreWeBackOnline("4.2.2.4");

                    await Task.Delay(DelayTime);

                    if (areWeBackOnline)
                    {
                        break;
                    }
                }
            }
        }

        private async Task KeepPinging(string hostName)
        {
            var pingTest = new PingTest(_consoleManager, hostName, PingTest.PingReportType.JustValue);

            while (true)
            {
                var result = await pingTest.Run();

                if (!result)
                {
                    return;
                }

                await Task.Delay(DelayTime);
            }
        }

        private Task<bool> AreWeBackOnline(string hostName)
        {
            var pingTest = new PingTest(_consoleManager, hostName, PingTest.PingReportType.NoReport);

            return pingTest.Run();
        }
    }
}