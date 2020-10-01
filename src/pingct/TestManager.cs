using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;
using Spectre.Console;

namespace Ctyar.Pingct
{
    internal class TestManager
    {
        private readonly int _delay;
        private readonly EventManager _eventManager;
        private readonly MainPingTest _mainPingTest;
        private readonly List<ReportPanel> _reportPanels;
        private readonly List<ITest> _tests;
        private readonly PanelManager _mainPanelManager;
        private readonly PanelManager _testPanelManager;
        private int _removeDelayCounter;

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

                    _removeDelayCounter = 4;
                }
                else if (!isOnline && wasOnline)
                {
#pragma warning disable 4014
                    Task.Run(() => RunTestsAsync(ct), ct);
#pragma warning restore 4014

                    _eventManager.Disconnected();
                }

                wasOnline = isOnline;

                await Task.Delay(_delay);
            }
        }

        private async Task RunTestsAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var tasks = _tests.Select(item => item.RunAsync()).ToList();

                await Task.WhenAll(tasks);

                foreach (var current in _tests)
                {
                    current.Report(_testPanelManager);
                }   
            }
        }

        private void RefreshConsole()
        {
            _mainPingTest.Report(_mainPanelManager);

            if (_removeDelayCounter > 0)
            {
                _removeDelayCounter--;
            }
            else if (_removeDelayCounter == 0)
            {
                _testPanelManager.Remove();
            }

            Console.SetCursorPosition(0, 0);

            AnsiConsole.Render(new Columns(_reportPanels.Select(item => item.Render())));
        }
    }
}