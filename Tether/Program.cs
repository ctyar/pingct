using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Tether
{
    class Program
    {
        private static WebClient WebClient = new WebClient(); 

        static async Task Main(string[] args)
        {
            while (true)
            {
                await TestGateway("192.168.1.1");

                // TODO: Needs improvment
                TestProxy();

                await TestIsp("91.98.29.182");

                TestDns();

                await TestOverseaServer("4.2.2.4");

                Console.WriteLine();

                Thread.Sleep(2000);
            }
        }

        private static void TestDns()
        {
            bool succeed = false;
            try
            {
                var ipHostEntry = Dns.GetHostEntry("www.google.com");
                succeed = ipHostEntry.AddressList.Length != 0;
            }
            catch
            {
                succeed = false;
            }

            Console.Write("DNS working: ");
            PrintResult(succeed);
        }

        private static void TestProxy()
        {
            bool result;
            try
            {
                using (WebClient.OpenRead("http://clients3.google.com/generate_204"))
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            Console.Write("Proxy working: ");
            PrintResult(result);
        }

        private async static Task TestGateway(string ip)
        {
            var ping = new Ping();

            var pingReply = await ping.SendPingAsync(ip);

            var success = pingReply.Status == IPStatus.Success;

            Console.Write("Reaching gateway: ");
            PrintResult(success);

            Console.Write("Gateway ping time: ");
            PrintValue(pingReply.RoundtripTime, 120, 170);
        }

        private async static Task TestOverseaServer(string host)
        {
            var ping = new Ping();

            var pingReply = await ping.SendPingAsync(host);

            var success = pingReply.Status == IPStatus.Success;

            Console.Write("Reaching oversea server: ");
            PrintResult(success);

            Console.Write("Oversea server ping time: ");
            PrintValue(pingReply.RoundtripTime, 120, 170);
        }

        private async static Task TestIsp(string ip)
        {
            var ping = new Ping();

            var pingReply = await ping.SendPingAsync(ip);

            var success = pingReply.Status == IPStatus.Success;

            Console.Write("Reaching ISP: ");
            PrintResult(success);

            Console.Write("ISP ping time: ");
            PrintValue(pingReply.RoundtripTime, 120, 170);
        }

        private static void PrintResult(bool isSuccess)
        {
            if (isSuccess)
            {
                PrintSucess("succeed");
            }
            else
            {
                PrintFailure("failed");
            }
        }

        private static void PrintValue(long value, long maxSuccessValue, long maxWarningValue)
        {
            if (value <= maxSuccessValue)
            {
                PrintSucess(value.ToString());
            }
            else if (value <= maxWarningValue)
            {
                PrintWarning(value.ToString());
            }
            else
            {
                PrintFailure(value.ToString());
            }
        }

        private static void PrintSucess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);

            Console.ResetColor();
        }

        private static void PrintFailure(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);

            Console.ResetColor();
        }

        private static void PrintWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);

            Console.ResetColor();
        }
    }
}
