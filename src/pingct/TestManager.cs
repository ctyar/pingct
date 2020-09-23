using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly PanelConsoleManager _mainConsoleManager;
        private readonly PanelConsoleManager _testConsoleManager;
        private int _removeDelayCounter = 0;

        public TestManager(EventManager eventManager, Settings settings, MainPingTest mainPingTest,
            IEnumerable<ITest> tests)
        {
            _eventManager = eventManager;
            _mainPingTest = mainPingTest;
            _tests = tests.ToList();
            _delay = settings.Delay;

            Console.CursorVisible = false;
            var lineCount = Console.WindowHeight - 4;

            var mainPanel = new ReportPanel(lineCount, "Ping");
            _mainConsoleManager = new PanelConsoleManager(mainPanel);

            var testsPanel = new ReportPanel(lineCount, "Tests");
            _testConsoleManager = new PanelConsoleManager(testsPanel);

            _reportPanels = new List<ReportPanel>
            {
                mainPanel,
                testsPanel
            };
        }

        public async Task ScanAsync()
        {
            var isOnline = true;

            while (true)
            {
                while (isOnline)
                {
                    isOnline = await _mainPingTest.RunAsync();

                    PrintMainPing();

                    await Task.Delay(_delay);
                }

                _eventManager.Disconnected();
                _removeDelayCounter = 4;

                while (!isOnline)
                {
                    isOnline = await _mainPingTest.RunAsync();

                    var tasks = _tests.Select(item => item.RunAsync()).ToList();

                    await Task.WhenAll(tasks);

                    PrintAll();

                    await Task.Delay(_delay);
                }

                _eventManager.Connected();
            }
        }

        private void PrintAll()
        {
            _mainPingTest.Report(_mainConsoleManager);

            foreach (var current in _tests)
            {
                current.Report(_testConsoleManager);
            }

            RefreshUi();
        }

        private void PrintMainPing()
        {
            _mainPingTest.Report(_mainConsoleManager);

            if (_removeDelayCounter > 0)
            {
                _removeDelayCounter--;
            }
            else if (_removeDelayCounter == 0)
            {
                _testConsoleManager.Remove();
            }

            RefreshUi();
        }

        private void RefreshUi()
        {
            Console.SetCursorPosition(0, 0);

            AnsiConsole.Render(new Columns(_reportPanels.Select(item => item.Render())));
        }
    }
}