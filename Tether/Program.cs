using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Tether
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Info("Application started.");

                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                var serviceProvider = serviceCollection.BuildServiceProvider();

                await serviceProvider.GetRequiredService<TestManager>().Scan();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                logger.Info("Exiting application.");
                LogManager.Shutdown();
            }
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", true, true)
                .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<Settings>(config.GetSection("Configuration"));

            serviceCollection.AddTransient<TestManager>()
                .AddTransient<IReportManager, ReportManager>()
                .AddTransient<ITest, GatewayTest>()
                .AddTransient<ITest, InCountryConnectionTest>()
                .AddTransient<ITest, DnsTest>()
                .AddTransient<ITest, FreedomTest>();

            serviceCollection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog(config);
            });
        }
    }
}