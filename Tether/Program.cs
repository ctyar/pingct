using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using SharpConfig;

namespace Tether
{
    internal class Program
    {
        private const string ConfigFileName = "config.cfg";
        private static readonly WebClient WebClient = new WebClient();

        private static async Task Main(string[] args)
        {
            Parser.Default.ParseArguments<Config>(args)
                .WithParsed(async opts => await SaveConfigAndScan(opts))
                .WithNotParsed(async errs => await LoadConfigAndScan());
        }

        private static async Task LoadConfigAndScan()
        {
            var config = LoadConfig();

            await Scan(config);
        }

        private static async Task SaveConfigAndScan(Config config)
        {
            SaveConfig(config);

            await Scan(config);
        }

        private static async Task Scan(Config config)
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

        private static Config LoadConfig()
        {
            var config = new Config();
            try
            {
                var configuration = Configuration.LoadFromFile(ConfigFileName);
                config = configuration[0].ToObject<Config>();
            }
            catch (FileNotFoundException)
            {
                PrintInfo("Config file not found. Loading default configurations.");
            }
            catch
            {
                PrintFailure("Loading config file failed. Loading default configurations.");
            }

            return config;
        }

        private static void SaveConfig(Config config)
        {
            var configuration = new Configuration();

            var props = typeof(Config).GetProperties().Where(
                prop => !Attribute.IsDefined(prop, typeof(IgnoreAttribute)));

            foreach (var propertyInfo in props)
            {
                configuration[nameof(Config)].Add(propertyInfo.Name, propertyInfo.GetValue(config));
            }

            try
            {
                configuration.SaveToFile(ConfigFileName);
            }
            catch
            {
                PrintFailure("Saving config file failed.");
            }
        }

        private static void TestDns()
        {
            var succeed = false;
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

        private static async Task TestGateway(string ip)
        {
            var ping = new Ping();

            var pingReply = await ping.SendPingAsync(ip);

            var success = pingReply.Status == IPStatus.Success;

            Console.Write("Reaching gateway: ");
            PrintResult(success);

            Console.Write("Gateway ping time: ");
            PrintValue(pingReply.RoundtripTime, 120, 170);
        }

        private static async Task TestOverseaServer(string host)
        {
            var ping = new Ping();

            var pingReply = await ping.SendPingAsync(host);

            var success = pingReply.Status == IPStatus.Success;

            Console.Write("Reaching oversea server: ");
            PrintResult(success);

            Console.Write("Oversea server ping time: ");
            PrintValue(pingReply.RoundtripTime, 120, 170);
        }

        private static async Task TestIsp(string ip)
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

        private static void PrintInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}