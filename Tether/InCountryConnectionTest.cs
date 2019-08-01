namespace Tether
{
    internal class InCountryConnectionTest : PingTest
    {
        public InCountryConnectionTest(IReportManager reportManager, string hostName) : base(reportManager, hostName,
            PingReportType.TestResult)
        {
        }
    }
}