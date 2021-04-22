namespace Ctyar.Pingct
{
    internal class Settings
    {
        public string Ping { get; set; } = "4.2.2.4";

        public int Delay { get; set; } = 1500;

        public long MaxPingSuccessTime { get; set; } = 120;

        public long MaxPingWarningTime { get; set; } = 170;

        public string Gateway { get; set; } = "192.168.1.1";

        public string InCountryHost { get; set; } = "aparat.com";

        public string Dns { get; set; } = "facebook.com";

        public string Get { get; set; } = "https://twitter.com";

        public string OnConnected { get; set; } = string.Empty;

        public string OnConnectedArgs { get; set; } = string.Empty;

        public string OnDisconnected { get; set; } = string.Empty;

        public string OnDisconnectedArgs { get; set; } = string.Empty;
    }
}