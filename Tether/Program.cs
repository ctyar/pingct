using System.Threading.Tasks;

namespace Tether
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            ITestManager testManager = new TestManager();

            var config = configurationManager.GetConfig(args);
            await testManager.Scan(config);
        }
    }
}