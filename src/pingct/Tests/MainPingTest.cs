namespace Ctyar.Pingct.Tests
{
    internal class MainPingTest : PingTest
    {
        public MainPingTest(Settings settings) : base(PingReportType.JustValue, settings.Ping,
            settings.MaxPingSuccessTime, settings.MaxPingWarningTime)
        {
        }
    }
}