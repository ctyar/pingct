﻿using System;
using System.Text.Json;

namespace Ctyar.Pingct;

internal class SettingsManager
{
    private const string SettingsFileName = "settings.json";

    private static readonly StorageManager StorageManager = new();

    public void Config()
    {
        var path = StorageManager.GetFilePath(SettingsFileName);

        Console.WriteLine(path);
    }

    public Settings Read()
    {
        Settings result;

        var fileContent = StorageManager.Read(SettingsFileName);

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

        StorageManager.Write(SettingsFileName, fileContent);
    }
}
