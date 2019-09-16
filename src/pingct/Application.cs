using System;
using System.Threading.Tasks;
using Serilog;

namespace Ctyar.Pingct
{
    internal class Application
    {
        private readonly ConfigManager _configManager;
        private readonly TestManager _testManager;

        public Application(TestManager testManager, ConfigManager configManager)
        {
            _testManager = testManager;
            _configManager = configManager;
        }

        public async Task Run()
        {
            try
            {
                await _testManager.Scan();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Stopped program because of exception");
                throw;
            }
        }

        public void Config()
        {
            try
            {
                _configManager.Config();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Stopped program because of exception");
                throw;
            }
        }
    }
}