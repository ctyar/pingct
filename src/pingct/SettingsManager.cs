using System;
using System.Text.Json;

namespace Ctyar.Pingct;

internal class SettingsManager
{
    private const string SettingsFileName = "settings.json";

    private readonly StorageManager _storageManager;

    public SettingsManager(StorageManager storageManager)
    {
        _storageManager = storageManager;
    }

    public void Config()
    {
        var path = _storageManager.GetFilePath(SettingsFileName);

        Console.WriteLine(path);
    }

    public Settings Read()
    {
        Settings result;

        var fileContent = _storageManager.Read(SettingsFileName);

        if (fileContent is null)
        {
            result = new Settings();

            SaveSettings(result);

            return result;
        }

        result = JsonSerializer.Deserialize<Settings>(fileContent)!;

        return result;
    }

    private void SaveSettings(Settings settings)
    {
        var fileContent = JsonSerializer.Serialize(settings, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        _storageManager.Write(SettingsFileName, fileContent);
    }
}