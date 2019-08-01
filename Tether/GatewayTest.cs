using Microsoft.Extensions.Options;

namespace Tether
{
    internal class GatewayTest : PingTest
    {
        public GatewayTest(IReportManager reportManager, IOptions<Settings> settings) :
            base(reportManager, PingReportType.TestResult, settings.Value.Gateway, settings.Value.MaxPingSuccessTime,
                settings.Value.MaxPingWarningTime)
        {
        }
    }
}