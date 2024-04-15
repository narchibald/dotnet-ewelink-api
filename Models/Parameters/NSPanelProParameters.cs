namespace EWeLink.Api.Models.Parameters
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class NSPanelProParameters : Parameters
    {
        [JsonProperty("rooted")]
        public bool Rooted { get; set; }

        [JsonProperty("cityId")]
        public string CityId { get; set; } = string.Empty;

        [JsonProperty("dst")]
        public int Destination { get; set; }

        [JsonProperty("dstChange")]
        public DateTime DestinationChange { get; set; }

        [JsonProperty("tempRange")]
        public string? TemperatureRange { get; set; }

        [JsonProperty("temperature")]
        public int Temperature { get; set; }

        [JsonProperty("timeZone")]
        public int TimeZone { get; set; }

        [JsonProperty("weather")]
        public int Weather { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("buzzerAlarm")]
        public BuzzerAlarm? BuzzerAlarm { get; set; }

        [JsonProperty("appVersion")]
        public string? ApplicationVersion { get; set; }

        [JsonProperty("sysVersion")]
        public string? SystemVersion { get; set; }

        [JsonProperty("zigbeeVersion")]
        public string? ZigbeeVersion { get; set; }

        [JsonProperty("securityType")]
        public int SecurityType { get; set; }

        [JsonProperty("subDevices")]
        public List<SubDevice> SubDevices { get; set; } = new ();

        [JsonProperty("geo")]
        public string? GeographicLocation { get; set; }

        [JsonProperty("addSubDevState")]
        public AddState AddSubDevState { get; set; } = AddState.Off;

        [JsonProperty("addDevTime")]
        public int AddDevTime { get; set; }

        [JsonProperty("screenList1")]
        public List<string> ScreenList1 { get; set; } = new ();

        [JsonProperty("screenList2")]
        public List<string> ScreenList2 { get; set; } = new ();

        [JsonProperty("screenList3")]
        public List<string> ScreenList3 { get; set; } = new ();

        [JsonProperty("securitySetting1")]
        public string? SecuritySetting1 { get; set; }

        [JsonProperty("securitySetting2")]
        public string? SecuritySetting2 { get; set; }

        [JsonProperty("securitySetting3")]
        public string? SecuritySetting3 { get; set; }
    }
}