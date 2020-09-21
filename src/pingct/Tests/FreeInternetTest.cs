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

        private readonly string _hostName;

        private bool _result;

        public FreeInternetTest()
        {
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

        public override void ReportCore(IConsoleManager consoleManager)
        {
            var (message, type) = _result ? ("OK", MessageType.Success) : ("Not working", MessageType.Failure);
            
            consoleManager.Print("Freedom: ", MessageType.Info);
            consoleManager.Print(message, type);
            consoleManager.PrintLine();
        }
    }
}