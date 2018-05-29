using CommandLine;

namespace Tether
{
    internal class Config
    {
        [SharpConfig.Ignore]
        [Option('c', "config", Required = true, HelpText = "Change the default configurations.")]
        public bool SetConfig { get; set; }

        [Option('o', "oversea", Required = true, HelpText = "Over sea server to ping.")]
        public string OverseaServer { get; set; } = "4.2.2.4";
    }
}