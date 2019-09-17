using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
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
                    Handler = CommandHandler.Create(async () => { await Run(); })
                };

                var configCommand = new Command("config")
                {
                    Handler = CommandHandler.Create(Config),
                    Description = "Run a wizard to change the default settings"
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

        private static async Task Run()
        {
            await GetServiceProvider().GetRequiredService<TestManager>().Scan();
        }

        private static void Config()
        {
            GetServiceProvider().GetRequiredService<SettingsManager>().RunWizard();
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
                    var configManager = provider.GetService<SettingsManager>();
                    var settings = configManager.Read();
                    return settings;
                })
                .AddTransient<TestManager>()
                .AddTransient<IConsoleManager, ConsoleManager>()
                .AddTransient<ITest, GatewayTest>()
                .AddTransient<ITest, InCountryConnectionTest>()
                .AddTransient<ITest, DnsTest>()
                .AddTransient<ITest, FreedomTest>();
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