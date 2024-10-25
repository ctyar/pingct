namespace Ctyar.Pingct;

internal enum TestRunType : byte
{
    Auto = 0,
    On = 1,
    Off = 2
}

internal static class TestRunTypeExtensions
{
    public static TestRunType Next(this TestRunType testRun)
    {
        var value = (int)testRun;

        value++;

        if (value > (int)TestRunType.Off)
        {
            value = 0;
        }

        return (TestRunType)value;
    }
}
