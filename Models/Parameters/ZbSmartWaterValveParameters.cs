namespace EWeLink.Api.Models.Parameters
{
    using System;
    using System.Collections.Generic;
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    public class ZbSmartWaterValveParameters : SnZbBatteryParameters
    {
        [JsonProperty("switch")]
        [JsonConverter(typeof(BoolSwitchConverter))]
        public SwitchState Switch { get; set; }

        [JsonProperty("lastIrrigationTime")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset? LastIrrigationTime { get; set; }

        [JsonProperty("endIrrigationTime")]
        [JsonConverter(typeof(UnixTimeMillisecondsConverter))]
        public DateTimeOffset? EndIrrigationTime { get; set; }

        [JsonProperty("realIrrigationVolume")]
        public int RealIrrigationVolume { get; set; }

        [JsonProperty("realIrrigationVolumeGal")]
        public decimal RealIrrigationVolumeGal { get; set; }

        [JsonProperty("todayWaterUsage")]
        public int TodayWaterUsage { get; set; }

        [JsonProperty("todayWaterUsageGal")]
        public decimal TodayWaterUsageGal { get; set; }

        [JsonProperty("hasException")]
        public bool HasException { get; set; }

        [JsonProperty("exceptionReport")]
        public List<string>? ExceptionReport { get; set; }

        [JsonProperty("controlMode")]
        public ControlMode ControlMode { get; set; }
    }
}