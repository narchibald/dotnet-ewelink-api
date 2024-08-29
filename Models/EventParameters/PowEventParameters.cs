namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public interface IPowEventParameters
        : IEventParameters
    {
        decimal Current { get; set; }

        decimal Voltage { get; set; }

        decimal Power { get; set; }

        decimal DayKwh { get; set; }

        decimal MonthKwh { get; set; }
    }

    [EventDeviceIdentifier(190)]
    public class PowEventParameters
        : EventParameters, IPowEventParameters
    {
        [JsonProperty("current")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Current { get; set; }

        [JsonProperty("voltage")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Voltage { get; set; }

        [JsonProperty("power")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Power { get; set; }

        [JsonProperty("dayKwh")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal DayKwh { get; set; }

        [JsonProperty("monthKwh")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal MonthKwh { get; set; }
    }
}