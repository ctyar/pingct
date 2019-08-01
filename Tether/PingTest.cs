using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tether
{
    internal class PingTest : ITest
    {
        private const long MaxPingSuccessTime = 120;
        private const long MaxPingWarningTime = 170;

        private readonly IReportManager _reportManager;
        private readonly string _hostName;
        private readonly PingReportType _reportType;

        private long _roundTripTime;

        public PingTest(IReportManager reportManager, string hostName, PingReportType reportType)
        {
            _reportManager = reportManager;
            _hostName = hostName;
            _reportType = reportType;
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
                _reportManager.Print(string.Empty, MessageType.Info);
                PrintPing(_hostName, _roundTripTime, MaxPingSuccessTime, MaxPingWarningTime);
            }
            else if (_reportType == PingReportType.JustValue)
            {
                PrintPing(_hostName, _roundTripTime, MaxPingSuccessTime, MaxPingWarningTime);
            }
        }

        private void PrintPing(string ip, long time, long maxSuccessTime, long maxWarningTime)
        {
            _reportManager.Print($"Reply from {ip}: time=", MessageType.Info);

            PrintPingValue(time, maxSuccessTime, maxWarningTime);

            _reportManager.Print("ms", MessageType.Info);

            _reportManager.PrintLine();
        }

        private void PrintPingValue(long value, long maxSuccessValue, long maxWarningValue)
        {
            if (value == 0)
            {
                _reportManager.Print(value.ToString(), MessageType.Failure);
            }
            else if (value <= maxSuccessValue)
            {
                _reportManager.Print(value.ToString(), MessageType.Success);
            }
            else if (value <= maxWarningValue)
            {
                _reportManager.Print(value.ToString(), MessageType.Warning);
            }
            else
            {
                _reportManager.Print(value.ToString(), MessageType.Failure);
            }
        }
    }
}