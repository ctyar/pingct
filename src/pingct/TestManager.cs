using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;
using Terminal.Gui;

namespace Ctyar.Pingct;

internal class TestManager
{
    private const int RemoveTestReportsDelay = 4;
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
    private TestRunType _testRunType;

    public TestManager(PanelManager pingPanelManager, PanelManager testPanelManager, Settings settings)
    {
        _mainPingTest = new(settings);
        _pingPanelManager = pingPanelManager;
        _testPanelManager = testPanelManager;
        _eventManager = new(settings);
        _delay = settings.Delay;
        _testsStopWatch = new();
        _tests = new TestFactory(settings).GetAll();
    }

    public async Task ScanAsync()
    {
        _isOnline = await _mainPingTest.RunAsync(CancellationToken.None);
        ReportPing();

        CheckCurrentStatus(_wasOnline, _isOnline);
        _wasOnline = _isOnline;
    }

    public void ToggleTests(TestRunType testRunType)
    {
        _testRunType = testRunType;

        if (!_isRunningTests && _testRunType == TestRunType.On)
        {
            Application.MainLoop.AddTimeout(TimeSpan.Zero, RunTestsHandler);

            _isRunningTests = true;
        }
        else if (_testRunType == TestRunType.Off)
        {
            _removeTestReportsDelayCounter = RemoveTestReportsDelay;
            _isRunningTests = false;
        }
    }

    private void CheckCurrentStatus(bool wasOnline, bool isOnline)
    {
        if (_testRunType == TestRunType.Auto)
        {
            if (!wasOnline && isOnline)
            {
                _eventManager.Connected();

                _isRunningTests = false;
                _removeTestReportsDelayCounter = RemoveTestReportsDelay;
            }
            else if (wasOnline && !isOnline)
            {
                Application.MainLoop.AddTimeout(TimeSpan.Zero, RunTestsHandler);

                _isRunningTests = true;

                _eventManager.Disconnected();
            }
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

        var tasks = _tests.Select(item => item.RunAsync(CancellationToken.None)).ToList();

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