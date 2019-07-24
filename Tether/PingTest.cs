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

        private readonly ConsoleManager _consoleManager;
        private readonly string _hostName;
        private readonly PingReportType _reportType;

        public PingTest(ConsoleManager consoleManager, string hostName, PingReportType reportType)
        {
            _consoleManager = consoleManager;
            _hostName = hostName;
            _reportType = reportType;
        }

        public async Task<bool> Run()
        {
            var ping = new Ping();
            var result = false;

            long roundTripTime;
            try
            {
                roundTripTime = (await ping.SendPingAsync(_hostName)).RoundtripTime;

                // Sometime the ping doesn't throw but it fails with zero roundtrip time
                if (roundTripTime != 0)
                {
                    result = true;
                }
            }
            catch (Exception e) when (e is PingException || e is SocketException)
            {
                roundTripTime = 0;
            }

            if (_reportType == PingReportType.TestResult)
            {
                _consoleManager.Print(string.Empty, MessageType.Info);
                _consoleManager.PrintPing(_hostName, roundTripTime, MaxPingSuccessTime, MaxPingWarningTime);
            }
            else if (_reportType == PingReportType.JustValue)
            {
                _consoleManager.PrintPing(_hostName, roundTripTime, MaxPingSuccessTime, MaxPingWarningTime);
            }

            return result;
        }

        public enum PingReportType
        {
            NoReport = 1,
            JustValue = 2,
            TestResult = 4
        }
    }
}