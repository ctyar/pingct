using System.Text.Json;

namespace Ctyar.Pingct
{
    internal class ConfigManager
    {
        private const string SettingsFileName = "settings.json";

        private readonly StorageManager _storageManager;

        public ConfigManager(StorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        public void Config()
        {
            var settings = new Settings();

            var fileContent = JsonSerializer.Serialize(settings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            _storageManager.Write(SettingsFileName, fileContent);
        }

        public Settings Read()
        {
            var fileContent = _storageManager.Read(SettingsFileName);

            if (fileContent is null)
            {
                return new Settings();
            }

            var settings = JsonSerializer.Deserialize<Settings>(fileContent);

            return settings;
        }
    }
}