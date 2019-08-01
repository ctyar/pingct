using Microsoft.Extensions.Options;

namespace Tether
{
    internal class InCountryConnectionTest : PingTest
    {
        public InCountryConnectionTest(IReportManager reportManager, IOptions<Settings> settings) :
            base(reportManager, PingReportType.TestResult, settings.Value.InCountryHost,
                settings.Value.MaxPingSuccessTime, settings.Value.MaxPingWarningTime)
        {
        }
    }
}