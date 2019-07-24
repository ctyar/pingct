namespace Tether
{
    internal class InCountryConnectionTest : PingTest
    {
        public InCountryConnectionTest(ConsoleManager consoleManager, string hostName) : base(consoleManager, hostName,
            PingReportType.TestResult)
        {
        }
    }
}