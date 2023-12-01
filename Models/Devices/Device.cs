namespace EWeLink.Api.Models.Devices
{
    using System;
    using System.Collections.Generic;

    using EWeLink.Api.Models.Converters;
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    [JsonConverter(typeof(DeviceConverter))]
    public abstract class Device : IDevice
    {
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [JsonProperty("family")]
        public Family? Family { get; set; }

        [JsonProperty("group")]
        public string? Group { get; set; }

        [JsonProperty("online")]
        public bool? Online { get; set; }

        [JsonProperty("shareUsers")]
        public List<object>? ShareUsers { get; set; }

        [JsonProperty("groups")]
        public List<object>? Groups { get; set; }

        [JsonProperty("devGroups")]
        public List<object> DevGroups { get; set; } = new ();

        [JsonProperty("deviceid")]
        public string? DeviceId { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("apikey")]
        public string? ApiKey { get; set; }

        [JsonProperty("extra")]
        public Extra? Extra { get; set; }

        [JsonProperty("settings")]
        public Settings? Settings { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("shareUsersInfo")]
        public List<object> ShareUsersInfo { get; set; } = new ();

        [JsonProperty("sharedTo")]
        public List<object> SharedTo { get; set; } = new ();

        [JsonProperty("__v")]
        public int V { get; set; }

        [JsonProperty("onlineTime")]
        public DateTime? OnlineTime { get; set; }

        [JsonProperty("ip")]
        public string? Ip { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("offlineTime")]
        public DateTime? OfflineTime { get; set; }

        [JsonProperty("devicekey")]
        public string? DeviceKey { get; set; }

        [JsonProperty("deviceUrl")]
        public string? DeviceUrl { get; set; }

        [JsonProperty("brandName")]
        public string? BrandName { get; set; }

        [JsonProperty("productModel")]
        public string? ProductModel { get; set; }

        [JsonProperty("showBrand")]
        public bool ShowBrand { get; set; }

        [JsonProperty("brandLogoUrl")]
        public string BrandLogoUrl { get; set; } = string.Empty;

        [JsonProperty("devConfig")]
        public DevConfig? DeviceConfiguration { get; set; }

        [JsonProperty("uiid")]
        public int Uiid { get; set; }

        [JsonProperty("denyFeatures")]
        public string[] DenyFeatures { get; set; }

        // Currently not populated from the fetched API data.
        public bool HasLanControl => this.LanControl != null;

        public LanControlInformation? LanControl { get; set; }
    }
}