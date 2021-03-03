namespace EWeLink.Api.Models
{
    public class SensorData
    {
        public SensorData(string deviceId, SensorType type, decimal value)
        {
            this.DeviceId = deviceId;
            this.Type = type;
            this.Value = value;
        }

        public string DeviceId { get; }

        public SensorType Type { get; }

        public decimal Value { get; }
    }
}