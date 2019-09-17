using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Ctyar.Pingct
{
    internal class PingTest : ITest
    {
        private readonly string _hostName;
        private readonly long _maxPingSuccessTime;
        private readonly long _maxPingWarningTime;
        private readonly IConsoleManager _consoleManager;
        private readonly PingReportType _reportType;

        private long _roundTripTime;

        public PingTest(IConsoleManager consoleManager, PingReportType reportType, string hostName,
            long maxPingSuccessTime, long maxPingWarningTime)
        {
            _consoleManager = consoleManager;
            _reportType = reportType;
            _hostName = hostName;
            _maxPingSuccessTime = maxPingSuccessTime;
            _maxPingWarningTime = maxPingWarningTime;
        }

        public async Task<bool> Run()
        {
            var result = false;
            Ping? ping = default;

            try
            {
                ping = new Ping();

                _roundTripTime = (await ping.SendPingAsync(_hostName)).RoundtripTime;

                // Sometime the ping doesn't throw but it fails with zero roundtrip time
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

        public void Report()
        {
            if (_reportType == PingReportType.TestResult)
            {
                _consoleManager.Print(string.Empty, MessageType.Info);
                PrintPing(_hostName, _roundTripTime, _maxPingSuccessTime, _maxPingWarningTime);
            }
            else if (_reportType == PingReportType.JustValue)
            {
                PrintPing(_hostName, _roundTripTime, _maxPingSuccessTime, _maxPingWarningTime);
            }
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
            if (value == 0)
            {
                _consoleManager.Print(value.ToString(), MessageType.Failure);
            }
            else if (value <= maxSuccessValue)
            {
                _consoleManager.Print(value.ToString(), MessageType.Success);
            }
            else if (value <= maxWarningValue)
            {
                _consoleManager.Print(value.ToString(), MessageType.Warning);
            }
            else
            {
                _consoleManager.Print(value.ToString(), MessageType.Failure);
            }
        }
    }
}