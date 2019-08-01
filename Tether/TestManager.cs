using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Tether
{
    internal class TestManager
    {
        private readonly int _delay;
        private readonly long _maxPingSuccessTime;
        private readonly long _maxPingWarningTime;
        private readonly string _ping;
        private readonly IReportManager _reportManager;
        private readonly IEnumerable<ITest> _tests;

        public TestManager(IEnumerable<ITest> tests, IReportManager reportManager, IOptions<Settings> settings)
        {
            _tests = tests;
            _reportManager = reportManager;
            _delay = settings.Value.Delay;
            _ping = settings.Value.Ping;
            _maxPingSuccessTime = settings.Value.MaxPingSuccessTime;
            _maxPingWarningTime = settings.Value.MaxPingWarningTime;
        }

        public async Task Scan()
        {
            while (true)
            {
                await KeepPinging(_ping);

                while (true)
                {
                    await RunAllTests();

                    PrintAllTests();

                    var areWeBackOnline = await AreWeBackOnline(_ping);

                    if (areWeBackOnline)
                    {
                        break;
                    }
                }
            }
        }

        private async Task RunAllTests()
        {
            var tasks = _tests.Select(item => item.Run()).ToList();

            await Task.WhenAll(tasks);
        }

        private void PrintAllTests()
        {
            foreach (var test in _tests)
            {
                _reportManager.Print("    ", MessageType.Info);
                test.Report();
            }
        }

        private async Task KeepPinging(string hostName)
        {
            var pingTest = new PingTest(_reportManager, PingReportType.JustValue, hostName, _maxPingSuccessTime,
                _maxPingWarningTime);

            while (true)
            {
                var isStillOnline = await pingTest.Run();

                if (!isStillOnline)
                {
                    return;
                }

                pingTest.Report();

                await Task.Delay(_delay);
            }
        }

        private Task<bool> AreWeBackOnline(string hostName)
        {
            var pingTest = new PingTest(_reportManager, PingReportType.NoReport, hostName, _maxPingSuccessTime,
                _maxPingWarningTime);

            return pingTest.Run();
        }
    }
}