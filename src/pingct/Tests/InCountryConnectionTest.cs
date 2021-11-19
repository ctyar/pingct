namespace Ctyar.Pingct.Tests;

internal class InCountryConnectionTest : PingTest
{
    public InCountryConnectionTest(Settings settings) : base(PingReportType.TestResult, settings.InCountryHost,
        settings.MaxPingSuccessTime, settings.MaxPingWarningTime)
    {
    }
}