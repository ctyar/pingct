using System;
using System.Threading.Tasks;
using Serilog;

namespace Tether
{
    internal class Application
    {
        private readonly TestManager _testManager;

        public Application(TestManager testManager)
        {
            _testManager = testManager;
        }

        public async Task Run()
        {
            try
            {
                Log.Information("Application started.");

                await _testManager.Scan();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                Log.Information("Exiting application.");
            }
        }
    }
}