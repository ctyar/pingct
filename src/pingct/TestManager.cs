﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly Stopwatch _testsStopWatch;
        private bool _isRunningTests;
        private int _removeTestReportsDelayCounter;
        private bool _isOnline;
        private bool _wasOnline = true;

        public TestManager(MainPingTest mainPingTest, PanelManager pingPanelManager, PanelManager testPanelManager,
            EventManager eventManager, IEnumerable<ITest> tests, Settings settings)
        {
            _mainPingTest = mainPingTest;
            _pingPanelManager = pingPanelManager;
            _testPanelManager = testPanelManager;
            _eventManager = eventManager;
            _tests = tests.ToList();
            _delay = settings.Delay;
            _testsStopWatch = new();
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

                _isRunningTests = false;
                _removeTestReportsDelayCounter = 4;
            }
            else if (wasOnline && !isOnline)
            {
                Application.MainLoop.AddTimeout(TimeSpan.Zero, RunTestsHandler);

                _isRunningTests = true;

                _eventManager.Disconnected();
            }
        }

        private bool RunTestsHandler(MainLoop mainLoop)
        {
            mainLoop.Invoke(async () =>
            {
                await RunTestsAsync();
            });

            return false;
        }

        private async Task RunTestsAsync()
        {
            _testsStopWatch.Restart();

            var tasks = _tests.Select(item => item.RunAsync(default)).ToList();

            await Task.WhenAll(tasks);

            foreach (var test in _tests)
            {
                test.Report(_testPanelManager);
            }

            _testsStopWatch.Stop();
            var currentDelay = _delay - _testsStopWatch.ElapsedMilliseconds;

            if (currentDelay > 0)
            {
                await Task.Delay((int)currentDelay);
            }

            if (_isRunningTests)
            {
                Application.MainLoop.AddTimeout(TimeSpan.Zero, RunTestsHandler);
            }
        }

        private void ReportPing()
        {
            _mainPingTest.Report(_pingPanelManager);

            if (!_isRunningTests)
            {
                if (_removeTestReportsDelayCounter > 0)
                {
                    _removeTestReportsDelayCounter--;
                }
                else
                {
                    _testPanelManager.Remove();
                }
            }
        }
    }
}