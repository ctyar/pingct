using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ctyar.Pingct
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            InitializeLogger();

            try
            {
                var rootCommand = new RootCommand
                {
                    Handler = CommandHandler.Create(async () => { await ScanAsync(); })
                };

                var configCommand = new Command("config")
                {
                    Handler = CommandHandler.Create(Config),
                    Description = "Prints the path to the config file"
                };

                rootCommand.Add(configCommand);
                return rootCommand.InvokeAsync(args).Result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Stopped program because of exception");
                throw;
            }
        }

        private static async Task ScanAsync()
        {
            await GetServiceProvider().GetRequiredService<TestManager>().ScanAsync();
        }

        private static void Config()
        {
            GetServiceProvider().GetRequiredService<SettingsManager>().Config();
        }

        private static ServiceProvider GetServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServices(serviceCollection);

            return serviceCollection.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<StorageManager>()
                .AddTransient<SettingsManager>()
                .AddSingleton(provider =>
                {
                    var settingsManager = provider.GetService<SettingsManager>();
                    var settings = settingsManager!.Read();
                    return settings;
                })
                .AddTransient<TestManager>()
                .AddTransient<EventManager>()
                .AddTransient<ProcessManager>()
                .AddTransient<MainPingTest>()
                .AddTransient<ITest, GatewayTest>()
                .AddTransient<ITest, InCountryConnectionTest>()
                .AddTransient<ITest, DnsTest>()
                .AddTransient<ITest, FreeInternetTest>();
        }

        private static void InitializeLogger()
        {
            var storageManager = new StorageManager();
            var filePath = storageManager.GetFilePath("log.txt");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(filePath)
                .CreateLogger();
        }
    }
}