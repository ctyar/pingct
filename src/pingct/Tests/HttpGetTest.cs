﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests
{
    internal class HttpGetTest : TestBase
    {
        private static readonly HttpClient HttpClient = new();

        private readonly string _hostName;

        private bool _result;

        public HttpGetTest(Settings settings)
        {
            _hostName = settings.Get;
        }

        public override async Task<bool> RunAsync(CancellationToken cancellationToken)
        {
            _result = false;

            try
            {
                var stream = await ExecuteWithTimeoutAsync(
                    async (ct) => await HttpClient.GetStreamAsync(_hostName),
                    cancellationToken
                );

                _result = true;
            }
            catch (Exception e) when (e is HttpRequestException || e is OperationCanceledException ||
                                      e is IOException || e is TimeoutRejectedException)
            {
            }

            return _result;
        }

        public override void Report(PanelManager panelManager)
        {
            var (message, type) = _result ? (_hostName, MessageType.Success) : (_hostName, MessageType.Failure);

            panelManager.Print("GET: ", MessageType.Info);
            panelManager.Print(message, type);
            panelManager.PrintLine();
        }
    }
}