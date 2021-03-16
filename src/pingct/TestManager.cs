using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;

namespace Ctyar.Pingct
{
    internal class TestManager
    {
        private readonly MainPingTest _mainPingTest;
        private readonly PanelManager _pingPanelManager;
        private readonly PanelManager _testPanelManager;
        private readonly EventManager _eventManager;
        private readonly List<ITest> _tests;
        private readonly int _delay;
        private int _removeDelayCounter;
        private bool _remove;

        public TestManager(MainPingTest mainPingTest, PanelManager pingPanelManager, PanelManager testPanelManager,
            EventManager eventManager, IEnumerable<ITest> tests, Settings settings)
        {
            _mainPingTest = mainPingTest;
            _pingPanelManager = pingPanelManager;
            _testPanelManager = testPanelManager;
            _eventManager = eventManager;
            _tests = tests.ToList();
            _delay = settings.Delay;
        }

        public async Task ScanAsync(CancellationToken cancellationToken)
        {
            var testsCancellationTokenSource = new CancellationTokenSource();

            var wasOnline = true;

            while (!cancellationToken.IsCancellationRequested)
            {
                var isOnline = await _mainPingTest.RunAsync(cancellationToken);
                ReportPing();

                testsCancellationTokenSource = CheckCurrentStatus(wasOnline, isOnline, testsCancellationTokenSource);
                wasOnline = isOnline;

                await Task.Delay(_delay, cancellationToken);
            }

            testsCancellationTokenSource.Cancel();
        }

        private CancellationTokenSource CheckCurrentStatus(bool wasOnline, bool isOnline,
            CancellationTokenSource cancellationTokenSource)
        {
            if (!wasOnline && isOnline)
            {
                _eventManager.Connected();

                _remove = true;
                _removeDelayCounter = 4;

                cancellationTokenSource.Cancel();
                return new CancellationTokenSource();
            }
            else if (wasOnline && !isOnline)
            {
                Task.Run(() => RunTestsAsync(cancellationTokenSource.Token), cancellationTokenSource.Token);

                _remove = false;

                _eventManager.Disconnected();
            }

            return cancellationTokenSource;
        }

        private async Task RunTestsAsync(CancellationToken cancellationToken)
        {
            var stopWatch = new Stopwatch();

            while (!cancellationToken.IsCancellationRequested)
            {
                stopWatch.Start();
                var tasks = _tests.Select(item => item.RunAsync(cancellationToken)).ToList();

                await Task.WhenAll(tasks);

                foreach (var current in _tests)
                {
                    current.Report(_testPanelManager);
                }

                stopWatch.Stop();
                if (stopWatch.ElapsedMilliseconds < _delay)
                {
                    await Task.Delay(_delay - (int)stopWatch.ElapsedMilliseconds, cancellationToken);
                }
                stopWatch.Reset();
            }
        }

        private void ReportPing()
        {
            _mainPingTest.Report(_pingPanelManager);

            if (_remove)
            {
                if (_removeDelayCounter > 0)
                {
                    _removeDelayCounter--;
                }
                else
                {
                    _testPanelManager.Remove();
                }
            }
        }
    }
}