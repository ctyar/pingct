namespace Ctyar.Pingct
{
    internal class InCountryConnectionTest : PingTest
    {
        public InCountryConnectionTest(IConsoleManager consoleManager, Settings settings) : base(consoleManager,
            PingReportType.TestResult, settings.InCountryHost, settings.MaxPingSuccessTime, settings.MaxPingWarningTime)
        {
        }
    }
}