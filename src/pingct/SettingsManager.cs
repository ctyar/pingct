using System.Text.Json;

namespace Ctyar.Pingct
{
    internal class SettingsManager
    {
        private const string SettingsFileName = "settings.json";

        private readonly StorageManager _storageManager;
        private readonly IConsoleManager _consoleManager;

        public SettingsManager(StorageManager storageManager, IConsoleManager consoleManager)
        {
            _storageManager = storageManager;
            _consoleManager = consoleManager;
        }

        public void Config()
        {
            var path = _storageManager.GetFilePath(SettingsFileName);

            _consoleManager.Print(path);
            _consoleManager.PrintLine();
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