namespace Ctyar.Pingct;

internal class Settings
{
    public string Ping { get; set; } = "4.2.2.4";

    public int Delay { get; set; } = 1500;

    public long MaxPingSuccessTime { get; set; } = 120;

    public long MaxPingWarningTime { get; set; } = 170;

    public TestSetting[] Tests { get; set; } =
    {
        new()
        {
            Type = TestType.Ping,
            Host = "192.168.0.1"
        },
        new()
        {
            Type = TestType.Ping,
            Host = "zi-tel.com"
        },
        new()
        {
            Type = TestType.Dns,
            Host = "facebook.com"
        },
        new()
        {
            Type = TestType.Get,
            Host = "https://twitter.com"
        }
    };

    public string OnConnected { get; set; } = string.Empty;

    public string OnConnectedArgs { get; set; } = string.Empty;

    public string OnDisconnected { get; set; } = string.Empty;

    public string OnDisconnectedArgs { get; set; } = string.Empty;
}

public class TestSetting
{
    public string? Type { get; set; }

    public string? Host { get; set; }
}