using System.Collections.Generic;
using CommandLine;

namespace Tether
{
    internal class Config
    {
        [SharpConfig.Ignore]
        [Option('c', "config", Required = true, HelpText = "Change the default configurations.")]
        public bool SetConfig { get; set; }

        [Option('t', "tests", Required = true, HelpText = "List of the tests.")]
        public IDictionary<string, string> Tests { get; set; } = new Dictionary<string, string>();
    }
}