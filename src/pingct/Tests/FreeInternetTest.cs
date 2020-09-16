using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Polly.Timeout;

namespace Ctyar.Pingct.Tests
{
    internal class FreeInternetTest : TestBase
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private readonly IConsoleManager _consoleManager;
        private readonly string _hostName;

        private bool _result;

        public FreeInternetTest(IConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
            _hostName = "https://twitter.com";
        }

        public override async Task<bool> RunCoreAsync()
        {
            _result = false;

            try
            {
                var stream = await ExecuteWithTimeoutAsync(
                    async () => await HttpClient.GetStreamAsync(_hostName)
                );

                _result = true;
            }
            catch (Exception e) when (e is HttpRequestException || e is OperationCanceledException ||
                                      e is IOException || e is TimeoutRejectedException)
            {
            }

            return _result;
        }

        public override void ReportCore()
        {
            var (message, type) = _result ? ("OK", MessageType.Success) : ("Not working", MessageType.Failure);
            
            _consoleManager.Print("Freedom: ", MessageType.Info);
            _consoleManager.Print(message, type);
            _consoleManager.PrintLine();
        }
    }
}