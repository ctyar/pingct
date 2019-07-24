﻿using System;
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

        private bool _result;

        public FreedomTest(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
            _hostName = Encoding.UTF8.GetString(Convert.FromBase64String("aHR0cHM6Ly9wb3JuaHViLmNvbQ=="));
        }

        public async Task<bool> Run()
        {
            _result = false;
            try
            {
                var stream = await HttpClient.GetStreamAsync(_hostName);
                _result = true;
            }
            catch (Exception e) when (e is HttpRequestException || e is ProxyException)
            {
            }

            return _result;
        }

        public void Report()
        {
            _consoleManager.Print("Freedom: ", MessageType.Info);
            _consoleManager.PrintResult(_result, "OK", "Not working");
        }
    }
}