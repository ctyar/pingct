using System.Collections.Generic;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;
using Terminal.Gui;

namespace Ctyar.Pingct
{
    internal class Gui
    {
        private readonly MainPingTest _mainPingTest;
        private readonly EventManager _eventManager;
        private readonly IEnumerable<ITest> _tests;
        private readonly Settings _settings;

        public Gui(MainPingTest mainPingTest, EventManager eventManager, IEnumerable<ITest> tests, Settings settings)
        {
            _mainPingTest = mainPingTest;
            _eventManager = eventManager;
            _tests = tests;
            _settings = settings;
        }

        public void Run()
        {
            var (pingPanel, testPanel) = InitializeUI();

            var pingPanelManager = new PanelManager(pingPanel);
            var testPanelManager = new PanelManager(testPanel);

            var testManager = new TestManager(_mainPingTest, pingPanelManager, testPanelManager, _eventManager, _tests,
                _settings);

            _ = Task.Run(() => testManager.ScanAsync(default));

            Application.Run();
        }

        private static (ReportPanel pingPanel, ReportPanel testPanel) InitializeUI()
        {
            Application.Init();

            var top = Application.Top;

            var attribute = Attribute.Make(Color.Gray, Color.Black);
            var mainColorScheme = new ColorScheme
            {
                Focus = attribute,
                HotFocus = attribute,
                HotNormal = attribute,
                Normal = attribute
            };

            var window = new Window("pingct")
            {
                X = -1,
                Y = -1,
                Width = Dim.Fill(-1),
                Height = Dim.Fill(-1),
                ColorScheme = mainColorScheme,
            };
            top.Add(window);

            var pingPanel = new ReportPanel("Ping")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Fill(),
                CanFocus = false
            };
            window.Add(pingPanel);

            var testPanel = new ReportPanel("Tests")
            {
                X = Pos.Right(pingPanel),
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Fill(),
                CanFocus = false
            };
            window.Add(testPanel);

            return (pingPanel, testPanel);
        }
    }
}