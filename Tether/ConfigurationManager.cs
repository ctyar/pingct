using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using SharpConfig;

namespace Tether
{
    internal interface IConfigurationManager
    {
        Config GetConfig(IEnumerable<string> args);
    }

    internal class ConfigurationManager : IConfigurationManager
    {
        private const string ConfigFileName = "config.cfg";
        private readonly IReportManager _reportManager;

        public ConfigurationManager() => _reportManager = new ConsoleReportManager();

        public Config GetConfig(IEnumerable<string> args)
        {
            var parserResult = Parser.Default.ParseArguments<Config>(args);

            if (parserResult is Parsed<Config> parsed)
            {
                SaveConfig(parsed.Value);

                return parsed.Value;
            }

            return LoadConfig();
        }

        private Config LoadConfig()
        {
            var config = new Config();
            try
            {
                var configuration = Configuration.LoadFromFile(ConfigFileName);
                config = configuration[0].ToObject<Config>();
            }
            catch (FileNotFoundException)
            {
                _reportManager.Report("Config file not found. Loading default configurations.", MessageType.Info);
            }
            catch
            {
                _reportManager.Report("Loading config file failed. Loading default configurations.",
                    MessageType.Failure);
            }

            return config;
        }

        private void SaveConfig(Config config)
        {
            var configuration = new Configuration();

            var props = typeof(Config).GetProperties().Where(
                prop => !Attribute.IsDefined(prop, typeof(IgnoreAttribute)));

            foreach (var propertyInfo in props)
            {
                configuration[nameof(Config)].Add(propertyInfo.Name, propertyInfo.GetValue(config));
            }

            try
            {
                configuration.SaveToFile(ConfigFileName);
            }
            catch
            {
                _reportManager.Report("Saving config file failed.", MessageType.Failure);
            }
        }
    }
}