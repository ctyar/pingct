namespace Tether
{
    internal class GatewayTest : PingTest
    {
        public GatewayTest(IReportManager reportManager, string hostName) : base(reportManager, hostName, PingReportType.TestResult)
        {
        }
    }
}