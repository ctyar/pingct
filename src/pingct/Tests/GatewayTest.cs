namespace Ctyar.Pingct.Tests;

internal class GatewayTest : PingTest
{
    public GatewayTest(Settings settings) : base(PingReportType.TestResult, settings.Gateway,
        settings.MaxPingSuccessTime, settings.MaxPingWarningTime)
    {
    }
}