using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;
using Terminal.Gui;

namespace Ctyar.Pingct
{
    /*internal class TestManager
    {
        private readonly int _delay;
        private readonly EventManager _eventManager;
        private readonly MainPingTest _mainPingTest;
        private readonly List<ReportPanel> _reportPanels;
        private readonly List<ITest> _tests;
        private readonly PanelManager _mainPanelManager;
        private readonly PanelManager _testPanelManager;
        private int _removeDelayCounter;
        private bool _remove;

        public TestManager(EventManager eventManager, Settings settings, MainPingTest mainPingTest,
            IEnumerable<ITest> tests)
        {
            _eventManager = eventManager;
            _mainPingTest = mainPingTest;
            _tests = tests.ToList();
            _delay = settings.Delay;

            var mainPanel = new ReportPanel("Ping");
            _mainPanelManager = new PanelManager(mainPanel);

            var testsPanel = new ReportPanel("Tests");
            _testPanelManager = new PanelManager(testsPanel);

            _reportPanels = new List<ReportPanel>
            {
                mainPanel,
                testsPanel
            };
        }

        public async Task ScanAsync()
        {
            var wasOnline = true;
            var tokenSource = new CancellationTokenSource();
            var ct = tokenSource.Token;

            while (true)
            {
                var isOnline = await _mainPingTest.RunAsync();

                RefreshConsole();

                if (isOnline && !wasOnline)
                {
                    _eventManager.Connected();
                    tokenSource.Cancel();

                    tokenSource = new CancellationTokenSource();
                    ct = tokenSource.Token;

                    _remove = true;
                    _removeDelayCounter = 4;
                }
                else if (!isOnline && wasOnline)
                {
#pragma warning disable 4014
                    Task.Run(() => RunTestsAsync(ct), ct);
#pragma warning restore 4014

                    _remove = false;

                    _eventManager.Disconnected();
                }

                wasOnline = isOnline;

                await Task.Delay(_delay);
            }
        }

        private async Task RunTestsAsync(CancellationToken ct)
        {
            var stopWatch = new Stopwatch();

            while (!ct.IsCancellationRequested)
            {
                stopWatch.Start();
                var tasks = _tests.Select(item => item.RunAsync()).ToList();

                await Task.WhenAll(tasks);

                foreach (var current in _tests)
                {
                    current.Report(_testPanelManager);
                }

                stopWatch.Stop();
                if (stopWatch.ElapsedMilliseconds < _delay)
                {
                    await Task.Delay(_delay - (int)stopWatch.ElapsedMilliseconds, ct);
                }
                stopWatch.Reset();
            }
        }

        private void RefreshConsole()
        {
            _mainPingTest.Report(_mainPanelManager);

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

            Console.SetCursorPosition(0, 0);

            AnsiConsole.Render(new Columns(_reportPanels.Select(item => item.Render())));
        }
    }*/

    internal class TestManager
    {
        private readonly MainPingTest _mainPingTest;
        private readonly PanelManager _pingPanelManager;
        private readonly PanelManager _testPanelManager;
        private readonly EventManager _eventManager;
        private readonly List<ITest> _tests;
        private readonly int _delay;

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
                _mainPingTest.Report(_pingPanelManager);

                testsCancellationTokenSource = RunTests(wasOnline, isOnline, testsCancellationTokenSource);
                wasOnline = isOnline;

                await Task.Delay(_delay, cancellationToken);
            }

            testsCancellationTokenSource.Cancel();
        }

        private CancellationTokenSource RunTests(bool wasOnline, bool isOnline, CancellationTokenSource cancellationTokenSource)
        {
            if (!wasOnline && isOnline)
            {
                _eventManager.Connected();

                cancellationTokenSource.Cancel();
                return new CancellationTokenSource();
            }
            else if (wasOnline  && !isOnline)
            {
                Task.Run(() => RunTestsAsync(cancellationTokenSource.Token), cancellationTokenSource.Token);

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
    }
}