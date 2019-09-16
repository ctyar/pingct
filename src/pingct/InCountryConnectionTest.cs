namespace Ctyar.Pingct
{
    internal class InCountryConnectionTest : PingTest
    {
        public InCountryConnectionTest(IReportManager reportManager, Settings settings) : base(reportManager,
            PingReportType.TestResult, settings.InCountryHost, settings.MaxPingSuccessTime, settings.MaxPingWarningTime)
        {
        }
    }
}