namespace Ctyar.Pingct.Tests
{
    internal class GatewayTest : PingTest
    {
        public GatewayTest(IConsoleManager consoleManager, Settings settings) : base(consoleManager,
            PingReportType.TestResult, settings.Gateway, settings.MaxPingSuccessTime, settings.MaxPingWarningTime)
        {
        }
    }
}