using System;
using Ctyar.Pingct.Tests;
using Terminal.Gui;

namespace Ctyar.Pingct;

internal class Gui
{
    private readonly MainPingTest _mainPingTest;
    private readonly EventManager _eventManager;
    private readonly TestFactory _testFactory;
    private readonly Settings _settings;
    private TestManager? _testManager;

    public Gui(MainPingTest mainPingTest, EventManager eventManager, TestFactory testFactory, Settings settings)
    {
        _mainPingTest = mainPingTest;
        _eventManager = eventManager;
        _testFactory = testFactory;
        _settings = settings;
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
            Width = Dim.Fill(0),
            Height = Dim.Fill(0),
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
            Width = Dim.Percent(50),
            Height = Dim.Fill(),
            CanFocus = false
        };
        window.Add(testPanel);

        Colors.ColorSchemes["Menu"].Disabled = attribute;
        var quitItem = new StatusItem(Key.CtrlMask | Key.Q, "~^Q~ Quit", () => QuitMenuItemHandler());
        var statusBar = new StatusBar(new StatusItem[] { quitItem });
        top.Add(statusBar);

        SetupMainLoop(pingPanel, testPanel);

        Application.Run();
    }

    private void SetupMainLoop(PanelView pingPanel, PanelView testPanel)
    {
        var pingPanelManager = new PanelManager(pingPanel);
        var testPanelManager = new PanelManager(testPanel);

        _testManager = new TestManager(_mainPingTest, pingPanelManager, testPanelManager, _eventManager, _testFactory, _settings);
        Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(_settings.Delay), MainLoopHandler);
    }

    private static void QuitMenuItemHandler()
    {
        Application.RequestStop();
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