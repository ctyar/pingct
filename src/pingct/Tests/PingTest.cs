using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ctyar.Pingct.Tests
{
    internal class PingTest : TestBase
    {
        private readonly IConsoleManager _consoleManager;
        private readonly PingReportType _reportType;

        private readonly string _hostName;
        private readonly long _maxPingSuccessTime;
        private readonly long _maxPingWarningTime;
        private long _roundTripTime;

        public PingTest(IConsoleManager consoleManager, PingReportType reportType, string hostName,
            long maxPingSuccessTime, long maxPingWarningTime) : base(consoleManager)
        {
            _consoleManager = consoleManager;
            _reportType = reportType;
            _hostName = hostName;
            _maxPingSuccessTime = maxPingSuccessTime;
            _maxPingWarningTime = maxPingWarningTime;
        }

        public override async Task<bool> RunCoreAsync()
        {
            var result = false;
            Ping? ping = default;

            try
            {
                ping = new Ping();

                _roundTripTime = (await ping.SendPingAsync(_hostName, 2000)).RoundtripTime;

                // Sometimes the ping doesn't throw but it fails with zero roundtrip time
                if (_roundTripTime != 0)
                {
                    result = true;
                }
            }
            catch (Exception e) when (e is PingException || e is SocketException)
            {
                _roundTripTime = 0;
            }
            finally
            {
                ping?.Dispose();
            }

            return result;
        }

        public override void ReportCore()
        {
            if (_reportType == PingReportType.NoReport)
            {
                return;
            }

            if (_reportType == PingReportType.TestResult)
            {
                _consoleManager.Print(string.Empty, MessageType.Info);
            }

            PrintPing(_hostName, _roundTripTime, _maxPingSuccessTime, _maxPingWarningTime);
        }

        private void PrintPing(string ip, long time, long maxSuccessTime, long maxWarningTime)
        {
            _consoleManager.Print($"Reply from {ip}: time=", MessageType.Info);

            PrintPingValue(time, maxSuccessTime, maxWarningTime);

            _consoleManager.Print("ms", MessageType.Info);

            _consoleManager.PrintLine();
        }

        private void PrintPingValue(long value, long maxSuccessValue, long maxWarningValue)
        {
            var messageType = value switch
            {
                0 => MessageType.Failure,
                _ when value <= maxSuccessValue => MessageType.Success,
                _ when value <= maxWarningValue => MessageType.Warning,
                _ => MessageType.Failure
            };

            _consoleManager.Print(value.ToString(), messageType);
        }
    }
}