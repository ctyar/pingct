using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Ctyar.Pingct.Tests;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Ctyar.Pingct;

internal class Program
{
    private static async Task Main(string[] args)
    {
        InitializeLogger();

        await BuildCommandLine()
            .UseDefaults()
            .UseExceptionHandler((exception, _) => Log.Error(exception, "Stopped program because of exception"))
            .Build()
            .InvokeAsync(args);
    }

    private static CommandLineBuilder BuildCommandLine()
    {
        var rootCommand = new RootCommand();
        rootCommand.SetHandler(() =>
        {
            GetServiceProvider().GetRequiredService<Gui>().Run();
        });

        var configCommand = new Command("config")
        {
            Description = "Prints the path to the config file"
        };
        configCommand.SetHandler(() =>
        {
            GetServiceProvider().GetRequiredService<SettingsManager>().Config();
        });
        rootCommand.Add(configCommand);

        return new CommandLineBuilder(rootCommand);
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
            .AddSingleton<StorageManager>()
            .AddSingleton<SettingsManager>()
            .AddSingleton(provider =>
            {
                var settingsManager = provider.GetService<SettingsManager>();
                var settings = settingsManager!.Read();
                return settings;
            })
            .AddSingleton<Gui>()
            .AddSingleton<EventManager>()
            .AddSingleton<ProcessManager>()
            .AddSingleton<MainPingTest>()
            .AddSingleton<TestFactory>();
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