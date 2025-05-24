using System;
using Terminal.Gui;

namespace Ctyar.Pingct;

internal class Tui
{
    private static readonly string[] TestStatusItemTitles = ["~SPACE~ Tests: Auto", "~SPACE~ Tests: On", "~SPACE~ Tests: Off"];

    private readonly Settings _settings;
    private TestManager? _testManager;
    private StatusItem _testStatusItem = null!;
    private TestRunType _testRunType;

    public Tui()
    {
        var settingsManager = new SettingsManager();
        _settings = settingsManager!.Read();
    }

    public void Run()
    {
        Application.Init();

        Application.Driver.SetCursorVisibility(CursorVisibility.Invisible);

        var top = Application.Top;

        var attribute = Terminal.Gui.Attribute.Make(Color.Gray, Color.Black);
        var mainColorScheme = new ColorScheme
        {
            Focus = attribute,
            HotFocus = attribute,
            HotNormal = attribute,
            Normal = attribute,
            Disabled = attribute,
        };

        var window = new Window("pingct")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ColorScheme = mainColorScheme,
            Border = new Border()
            {
                BorderStyle = BorderStyle.None,
                DrawMarginFrame = false,
                Effect3D = false,
            }
        };
        top.Add(window);

        var pingPanel = new PanelView("Ping")
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(50),
            Height = Dim.Fill(),
            CanFocus = false
        };
        window.Add(pingPanel);

        var testPanel = new PanelView("Tests")
        {
            X = Pos.Right(pingPanel),
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            CanFocus = false
        };
        window.Add(testPanel);

        var quitItem = new StatusItem(Key.CtrlMask | Key.Q, "~^Q~ Quit", QuitMenuItemHandler);
        _testStatusItem = new StatusItem(Key.Space, TestStatusItemTitles[0], TestMenuItemHandler);
        var statusBar = new StatusBar([quitItem, _testStatusItem])
        {
            ColorScheme = new ColorScheme
            {
                HotNormal = Terminal.Gui.Attribute.Make(Color.BrightYellow, Color.Black),
                Normal = Terminal.Gui.Attribute.Make(Color.White, Color.Black),
            }
        };
        top.Add(statusBar);

        SetupMainLoop(pingPanel, testPanel);

        Application.Run();
    }

    private void SetupMainLoop(PanelView pingPanel, PanelView testPanel)
    {
        var pingPanelManager = new PanelManager(pingPanel);
        var testPanelManager = new PanelManager(testPanel);

        _testManager = new TestManager(pingPanelManager, testPanelManager, _settings);
        Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(_settings.Delay), MainLoopHandler);
    }

    private static void QuitMenuItemHandler()
    {
        Application.RequestStop();
    }

    private void TestMenuItemHandler()
    {
        _testRunType = _testRunType.Next();

        _testStatusItem.Title = TestStatusItemTitles[(int)_testRunType];
        _testManager!.ToggleTests(_testRunType);
    }

    private bool MainLoopHandler(MainLoop mainLoop)
    {
        mainLoop.Invoke(async () =>
        {
            await _testManager!.ScanAsync();
        });

        return true;
    }
}