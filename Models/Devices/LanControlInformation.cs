namespace EWeLink.Api.Models.Devices
{
    using System.Net;

    public class LanControlInformation
    {
        public LanControlInformation(IPAddress ipAddress, int port, bool encryptionEnabled)
        {
            IpAddress = ipAddress;
            Port = port;
            EncryptionEnabled = encryptionEnabled;
        }

        public IPAddress IpAddress { get; }

        public int Port { get; }

        public bool EncryptionEnabled { get; }
    }
}