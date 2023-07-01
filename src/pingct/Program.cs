using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
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
            new Tui().Run();
            //GetServiceProvider().GetRequiredService<Gui>().Run();
        });

        var configCommand = new Command("config")
        {
            Description = "Prints the path to the config file"
        };
        configCommand.SetHandler(() =>
        {
            new SettingsManager().Config();
        });
        rootCommand.Add(configCommand);

        return new CommandLineBuilder(rootCommand);
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