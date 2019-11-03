using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ctyar.Pingct
{
    internal class TestManager
    {
        private readonly int _delay;
        private readonly long _maxPingSuccessTime;
        private readonly long _maxPingWarningTime;
        private readonly string _ping;
        private readonly IConsoleManager _consoleManager;
        private readonly EventManager _eventManager;
        private readonly IEnumerable<ITest> _tests;

        public TestManager(IEnumerable<ITest> tests, IConsoleManager consoleManager, EventManager eventManager, Settings settings)
        {
            _tests = tests;
            _consoleManager = consoleManager;
            _eventManager = eventManager;
            _delay = settings.Delay;
            _ping = settings.Ping;
            _maxPingSuccessTime = settings.MaxPingSuccessTime;
            _maxPingWarningTime = settings.MaxPingWarningTime;
        }

        public async Task ScanAsync()
        {
            while (true)
            {
                await KeepPingingAsync(_ping);

                _eventManager.Disconnected();

                while (true)
                {
                    await RunAllTestsAsync();

                    PrintAllTests();

                    var areWeBackOnline = await AreWeBackOnline(_ping);

                    if (areWeBackOnline)
                    {
                        _eventManager.Connected();
                        break;
                    }
                }
            }
        }

        private async Task RunAllTestsAsync()
        {
            var tasks = _tests.Select(item => item.RunAsync()).ToList();

            await Task.WhenAll(tasks);
        }

        private void PrintAllTests()
        {
            foreach (var test in _tests)
            {
                _consoleManager.Print("    ", MessageType.Info);
                test.Report();
            }
        }

        private async Task KeepPingingAsync(string hostName)
        {
            var pingTest = new PingTest(_consoleManager, PingReportType.JustValue, hostName, _maxPingSuccessTime,
                _maxPingWarningTime);

            while (true)
            {
                var isStillOnline = await pingTest.RunAsync();

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
            var pingTest = new PingTest(_consoleManager, PingReportType.NoReport, hostName, _maxPingSuccessTime,
                _maxPingWarningTime);

            return pingTest.RunAsync();
        }
    }
}