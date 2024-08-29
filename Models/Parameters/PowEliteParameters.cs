namespace EWeLink.Api.Models.Parameters
{
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public class PowEliteParameters : OneSwitchParameters
    {
        [JsonProperty("threshold")]
        public PowerThresholds Thresholds { get; set; }

        [JsonProperty("overload_00")]
        public OverloadThresholds Overloads{ get; set; }

        [JsonProperty("current")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Current { get; set; }

        [JsonProperty("voltage")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Voltage { get; set; }

        [JsonProperty("power")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Power { get; set; }

        [JsonProperty("timeZone")]
        public decimal? TimeZone { get; set; }

        [JsonProperty("dayKwh")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal DayKwh { get; set; }

        [JsonProperty("monthKwh")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal MonthKwh { get; set; }

        [JsonProperty("uiActive")]
        public int UiActive { get; set; }

        [JsonProperty("getHoursKwh")]
        public HoursKwh GetHoursKwh { get; set; }
    }

    public class OverloadThresholds
    {
        [JsonProperty("minAP")]
        public OverloadThreshold MinActivePower { get; set; }

        [JsonProperty("maxAP")]
        public OverloadThreshold MaxActivePower { get; set; }

        [JsonProperty("minV")]
        public OverloadThreshold MinVoltage { get; set; }

        [JsonProperty("maxV")]
        public OverloadThreshold MaxVoltage { get; set; }

        [JsonProperty("maxC")]
        public OverloadThreshold MinCurrent { get; set; }

        [JsonProperty("minC")]
        public OverloadThreshold MaxCurrent { get; set; }
    }

    public class OverloadThreshold
    {
        [JsonProperty("en")]
        [JsonConverter(typeof(BoolConverter))]
        public bool Enabled { get; set; }

        [JsonProperty("val")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Value { get; set; }
    }

    public class PowerThreshold
    {
        [JsonProperty("min")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Min { get; set; }

        [JsonProperty("max")]
        [JsonConverter(typeof(IntToDecimalConverter))]
        public decimal Max { get; set; }
    }

    public class PowerThresholds
    {
        [JsonProperty("actPow")]
        public PowerThreshold ActivePower { get; set; }

        [JsonProperty("voltage")]
        public PowerThreshold Voltage { get; set; }

        [JsonProperty("current")]
        public PowerThreshold Current { get; set; }
    }

    public class HoursKwh
    {
        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }
    }
}
