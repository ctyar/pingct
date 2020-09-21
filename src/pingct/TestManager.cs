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
        private readonly ReportPanel _mainPanel;
        private readonly MainPingTest _mainPingTest;
        private readonly List<ReportPanel> _reportPanels;
        private readonly List<ITest> _tests;
        private readonly ReportPanel _testsPanel;

        public TestManager(EventManager eventManager, Settings settings, MainPingTest mainPingTest,
            IEnumerable<ITest> tests)
        {
            _eventManager = eventManager;
            _mainPingTest = mainPingTest;
            _tests = tests.ToList();
            _delay = settings.Delay;

            Console.CursorVisible = false;
            var lineCount = Console.WindowHeight - 4;

            _mainPanel = new ReportPanel(lineCount, "Ping");
            _testsPanel = new ReportPanel(lineCount, "Tests");

            _reportPanels = new List<ReportPanel>
            {
                _mainPanel,
                _testsPanel
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

                    PrintTest();

                    await Task.Delay(_delay);
                }

                _eventManager.Disconnected();

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
            _mainPingTest.Report(new PanelConsoleManager(_mainPanel));

            foreach (var current in _tests)
            {
                current.Report(new PanelConsoleManager(_testsPanel));
            }

            RefreshUi();
        }

        private void PrintTest()
        {
            _mainPingTest.Report(new PanelConsoleManager(_mainPanel));

            RefreshUi();
        }

        private void RefreshUi()
        {
            Console.SetCursorPosition(0, 0);

            AnsiConsole.Render(new Columns(_reportPanels.Select(item => item.Render())));
        }
    }
}