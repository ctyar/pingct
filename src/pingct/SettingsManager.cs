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

        public void RunWizard()
        {
            var settings = Read();

            _consoleManager.Print("Please enter the following values (leave empty for default):");
            _consoleManager.PrintLine();

            settings.Ping = ReadValue("Ping host name or IP address", settings.Ping);
            settings.Gateway = ReadValue("Gateway host name or IP address", settings.Gateway);
            settings.InCountryHost = ReadValue("A host name or IP address inside the country", settings.InCountryHost);
            settings.Dns = ReadValue("A host name or IP address for DNS test", settings.Dns);

            SaveSettings(settings);
        }

        private void SaveSettings(Settings settings)
        {
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

        private string ReadValue(string message, string oldValue)
        {
            _consoleManager.Print($"{message}(default = {oldValue}): ");

            var newValue = _consoleManager.ReadLine();
            
            if (!string.IsNullOrEmpty(newValue))
            {
                return newValue;
            }

            return oldValue;
        }

        private int ReadValue(string message, int oldValue, int minimumValidValue)
        {
            do
            {
                _consoleManager.Print($"{message}(default = {oldValue}): ");

                var newStringValue = _consoleManager.ReadLine();

                if (string.IsNullOrEmpty(newStringValue))
                {
                    return oldValue;
                }

                var valid = int.TryParse(newStringValue, out var newValue);

                if (valid && newValue >= minimumValidValue)
                {
                    return newValue;
                }

                _consoleManager.Print("Invalid value");
            } while (true);
        }
    }
}