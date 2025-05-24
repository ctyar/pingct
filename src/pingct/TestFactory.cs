using System.Collections.Generic;
using Ctyar.Pingct.Tests;

namespace Ctyar.Pingct;

internal class TestFactory
{
    private readonly Settings _settings;

    public TestFactory(Settings settings)
    {
        _settings = settings;
    }

    public List<ITest> GetAll()
    {
        var result = new List<ITest>();

        foreach (var test in _settings.Tests)
        {
            if (test.Type is null || test.Host is null)
            {
                continue;
            }

            ITest? testObject = test.Type.ToLower() switch
            {
                TestType.Ping => new PingTest(PingReportType.TestResult, test.Host, _settings.MaxPingSuccessTime, _settings.MaxPingWarningTime),
                TestType.Dns => new DnsTest(test.Host),
                TestType.Get => new HttpGetTest(test.Host),
                _ => null
            };

            if (testObject is null)
            {
                continue;
            }

            result.Add(testObject);
        }

        return result;
    }
}