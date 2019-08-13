using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ctyar.Pingct
{
    internal class Program
    {
        private static async Task Main(string[] _)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider.GetRequiredService<Application>().Run();
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(Settings.FileName, true, true)
                .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<Settings>(config.GetSection("Configuration"));

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt")
                .CreateLogger();

            serviceCollection
                .AddTransient<Application>()
                .AddTransient<TestManager>()
                .AddTransient<IReportManager, ReportManager>()
                .AddTransient<ITest, GatewayTest>()
                .AddTransient<ITest, InCountryConnectionTest>()
                .AddTransient<ITest, DnsTest>()
                .AddTransient<ITest, FreedomTest>();
        }
    }
}