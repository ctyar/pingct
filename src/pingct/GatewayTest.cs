namespace Ctyar.Pingct
{
    internal class GatewayTest : PingTest
    {
        public GatewayTest(IReportManager reportManager, Settings settings) : base(reportManager,
            PingReportType.TestResult, settings.Gateway, settings.MaxPingSuccessTime, settings.MaxPingWarningTime)
        {
        }
    }
}