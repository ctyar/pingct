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
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ColorScheme = mainColorScheme,
            };
            top.Add(window);

            var pingPanel = new ReportPanel("Ping")
            {
                X = 0,
                Y = 1,
                Width = Dim.Percent(50),
                Height = Dim.Fill(),
                CanFocus = false
            };
            window.Add(pingPanel);

            // Subpanel layout
            /*var testsView = new View
            {
                X = Pos.Right(pingPanel),
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                CanFocus = false
            };

            window.Add(testsView);

            var panel1 = new ReportPanel("Panel1")
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Percent(50),
                CanFocus = false
            };
            var panel2 = new ReportPanel("Panel2")
            {
                X = 0,
                Y = Pos.Bottom(panel1),
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                CanFocus = false
            };
            testsView.Add(panel1);
            testsView.Add(panel2);*/

            var testPanel = new ReportPanel("Tests")
            {
                X = Pos.Right(pingPanel),
                Y = 1,
                Width = Dim.Percent(50),
                Height = Dim.Fill(),
                CanFocus = false
            };
            window.Add(testPanel);

            return (pingPanel, testPanel);
        }
    }
}