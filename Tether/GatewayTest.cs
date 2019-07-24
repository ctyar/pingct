namespace Tether
{
    internal class GatewayTest : PingTest
    {
        public GatewayTest(ConsoleManager consoleManager, string hostName) : base(consoleManager, hostName, PingReportType.TestResult)
        {
        }
    }
}