namespace EWeLink.Api.Models
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Paramaters
    {
        [JsonProperty("init")]
        public int Init { get; set; }

        [JsonProperty("rssi")]
        public int Rssi { get; set; }

        [JsonProperty("staMac")]
        public string StaMac { get; set; }

        [JsonProperty("timers")]
        public List<Timer> Timers { get; set; }

        [JsonProperty("pulseWidth")]
        public int PulseWidth { get; set; }

        [JsonProperty("swMode")]
        public int SwMode { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("switch")]
        public SwitchState? Switch { get; set; }

        [JsonProperty("switches")]
        public LinkSwitch?[] Switches { get; set; }

        [JsonProperty("bindInfos")]
        public BindInfos BindInfos { get; set; }

        [JsonProperty("startup")]
        public string? Startup { get; set; }

        [JsonProperty("pulse")]
        public string? Pulse { get; set; }

        [JsonProperty("sledOnline")]
        public string? SledOnline { get; set; }

        [JsonProperty("fwVersion")]
        public string? FirmWareVersion { get; set; }

        [JsonProperty("partnerApikey")]
        public string? PartnerApikey { get; set; }

        [JsonProperty("currentTemperature")]
        [JsonConverter(typeof(SensorJsonConverter))]
        public double? CurrentTemperature { get; set; }

        [JsonProperty("currentHumidity")]
        [JsonConverter(typeof(SensorJsonConverter))]
        public double? CurrentHumidity { get; set; }
    }
}