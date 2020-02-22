namespace Ctyar.Pingct.Tests
{
    internal class InCountryConnectionTest : PingTest
    {
        public InCountryConnectionTest(IConsoleManager consoleManager, Settings settings) : base(consoleManager,
            PingReportType.TestResult, settings.InCountryHost, settings.MaxPingSuccessTime, settings.MaxPingWarningTime)
        {
        }
    }
}