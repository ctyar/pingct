using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ctyar.Pingct.Tests;

internal class PingTest : TestBase
{
    private readonly string _hostName;
    private readonly long _maxPingSuccessTime;
    private readonly long _maxPingWarningTime;
    private readonly PingReportType _reportType;
    private long _roundTripTime;

    public override string Name => "Ping";

    public PingTest(PingReportType reportType, string hostName, long maxPingSuccessTime, long maxPingWarningTime)
    {
        _reportType = reportType;
        _hostName = hostName;
        _maxPingSuccessTime = maxPingSuccessTime;
        _maxPingWarningTime = maxPingWarningTime;
    }

    public override async Task<bool> RunAsync(CancellationToken cancellationToken)
    {
        var result = false;
        Ping? ping = null;

        try
        {
            ping = new();

            _roundTripTime = (await ping.SendPingAsync(_hostName, 2000)).RoundtripTime;

            // Sometimes the ping doesn't throw but it fails with zero roundtrip time
            if (_roundTripTime != 0)
            {
                result = true;
            }
        }
        catch (Exception e) when (e is PingException or SocketException)
        {
            _roundTripTime = 0;
        }
        finally
        {
            ping?.Dispose();
        }

        return result;
    }

    public override void Report(PanelManager panelManager)
    {
        if (_reportType == PingReportType.TestResult)
        {
            panelManager.Print(string.Empty, MessageType.Info);
        }

        PrintPing(_hostName, _roundTripTime, _maxPingSuccessTime, _maxPingWarningTime, panelManager);
    }

    private static void PrintPing(string ip, long time, long maxSuccessTime, long maxWarningTime,
        PanelManager panelManager)
    {
        panelManager.Print($"Reply from {ip}: time=", MessageType.Info);

        PrintPingValue(time, maxSuccessTime, maxWarningTime, panelManager);

        panelManager.Print("ms", MessageType.Info);

        panelManager.PrintLine();
    }

    private static void PrintPingValue(long value, long maxSuccessValue, long maxWarningValue,
        PanelManager panelManager)
    {
        var messageType = value switch
        {
            0 => MessageType.Failure,
            _ when value <= maxSuccessValue => MessageType.Success,
            _ when value <= maxWarningValue => MessageType.Warning,
            _ => MessageType.Failure
        };

        panelManager.Print(value.ToString(), messageType);
    }
}
