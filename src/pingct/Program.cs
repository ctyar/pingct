using System;
using System.CommandLine;
using Serilog;

namespace Ctyar.Pingct;

internal class Program
{
    private static int Main(string[] args)
    {
        InitializeLogger();

        var rootCommand = BuildCommandLine();

        var parseResult = rootCommand.Parse(args);

        if (parseResult.Errors.Count > 0)
        {
            foreach (var parseError in parseResult.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
            }

            return 1;
        }

        parseResult.Invoke();

        return 0;
    }

    private static RootCommand BuildCommandLine()
    {
        var rootCommand = new RootCommand();

        rootCommand.SetAction(parseResult =>
        {
            try
            {
                new Tui().Run();
            }
            catch (Exception e)
            {
                Log.Error(e, "Stopped program because of exception");

                throw;
            }
        });

        var configCommand = new Command("config")
        {
            Description = "Prints the path to the config file"
        };
        configCommand.SetAction(parseResult =>
        {
            try
            {
                new SettingsManager().Config();
            }
            catch (Exception e)
            {
                Log.Error(e, "Stopped program because of exception");

                throw;
            }
        });
        rootCommand.Add(configCommand);

        return rootCommand;
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