using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;
using Terminal.Gui;

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
        private bool _isOnline;
        private bool _wasOnline = true;
        private object? _testsToken;

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

        public async Task ScanAsync()
        {
            _isOnline = await _mainPingTest.RunAsync();
            ReportPing();

            CheckCurrentStatus(_wasOnline, _isOnline);
            _wasOnline = _isOnline;
        }

        private void CheckCurrentStatus(bool wasOnline, bool isOnline)
        {
            if (!wasOnline && isOnline)
            {
                _eventManager.Connected();

                _remove = true;
                _removeDelayCounter = 4;

                Application.MainLoop.RemoveTimeout(_testsToken);
            }
            else if (wasOnline && !isOnline)
            {
                _testsToken = Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(_delay), RunTestsHandler);

                _remove = false;

                _eventManager.Disconnected();
            }
        }

        private bool RunTestsHandler(MainLoop mainLoop)
        {
            mainLoop.Invoke(async () =>
            {
                await RunTestsAsync();
            });

            return true;
        }

        private async Task RunTestsAsync()
        {
            var tasks = _tests.Select(item => item.RunAsync(default)).ToList();

            await Task.WhenAll(tasks);

            foreach (var test in _tests)
            {
                test.Report(_testPanelManager);
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