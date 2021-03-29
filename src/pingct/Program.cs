using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ctyar.Pingct
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            InitializeLogger();

            await BuildCommandLine()
                .UseDefaults()
                .UseExceptionHandler((exception, context) => Log.Error(exception, "Stopped program because of exception"))
                .Build()
                .InvokeAsync(args);
        }

        private static CommandLineBuilder BuildCommandLine()
        {
            var rootCommand = new RootCommand
            {
                Handler = CommandHandler.Create(Scan)
            };

            var configCommand = new Command("config")
            {
                Handler = CommandHandler.Create(Config),
                Description = "Prints the path to the config file"
            };
            rootCommand.Add(configCommand);

            return new CommandLineBuilder(rootCommand);
        }

        private static void Scan()
        {
            GetServiceProvider().GetRequiredService<Gui>().Run();
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
                .AddTransient<Gui>()
                .AddTransient<EventManager>()
                .AddTransient<ProcessManager>()
                .AddTransient<MainPingTest>()
                .AddTransient<ITest, GatewayTest>()
                .AddTransient<ITest, InCountryConnectionTest>()
                .AddTransient<ITest, DnsTest>()
                .AddTransient<ITest, HttpGetTest>();
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