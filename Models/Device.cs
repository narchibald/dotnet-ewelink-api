namespace EWeLink.Api.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Device
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
        public List<object>? DevGroups { get; set; }

        [JsonProperty("deviceid")]
        public string? Deviceid { get; set; }

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

        [JsonProperty("params")]
        public Paramaters Paramaters { get; set; } = null!;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("shareUsersInfo")]
        public List<object> ShareUsersInfo { get; set; } = null!;

        [JsonProperty("__v")]
        public int V { get; set; }

        [JsonProperty("onlineTime")]
        public DateTime? OnlineTime { get; set; }

        [JsonProperty("ip")]
        public string? Ip { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("tags")]
        public Tags? Tags { get; set; }

        [JsonProperty("offlineTime")]
        public DateTime? OfflineTime { get; set; }

        [JsonProperty("deviceUrl")]
        public string? DeviceUrl { get; set; }

        [JsonProperty("brandName")]
        public string? BrandName { get; set; }

        [JsonProperty("productModel")]
        public string? ProductModel { get; set; }

        [JsonProperty("uiid")]
        public int Uiid { get; set; }

    }
}