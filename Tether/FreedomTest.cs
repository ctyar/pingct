using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SocksSharp;
using SocksSharp.Proxy;

namespace Tether
{
    internal class FreedomTest : ITest
    {
        private static readonly ProxySettings ProxySettings = new ProxySettings
        {
            Host = "127.0.0.1",
            Port = 9150
        };

        private static readonly ProxyClientHandler<Socks5> ProxyClientHandler =
            new ProxyClientHandler<Socks5>(ProxySettings);

        private static readonly HttpClient HttpClient = new HttpClient(ProxyClientHandler);

        private readonly ConsoleManager _consoleManager;
        private readonly string _hostName;

        public FreedomTest(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
            _hostName = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cHM6Ly9wb3JuaHViLmNvbQ=="));
        }

        public async Task<bool> Run()
        {
            var result = false;
            try
            {
                var stream = await HttpClient.GetStreamAsync(_hostName);
                result = true;
            }
            catch (Exception e) when (e is HttpRequestException || e is ProxyException)
            {
            }

            _consoleManager.Print("Freedom: ", MessageType.Info);
            _consoleManager.PrintResult(result, "OK", "Not working");

            return result;
        }
    }
}