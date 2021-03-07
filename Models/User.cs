namespace EWeLink.Api.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class User
    {
        [JsonProperty("clientInfo")]
        public ClientInfo ClientInfo { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("appId")]
        public string AppId { get; set; }

        [JsonProperty("apikey")]
        public string Apikey { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("__v")]
        public int V { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("onlineTime")]
        public DateTime OnlineTime { get; set; }

        [JsonProperty("appInfos")]
        public List<AppInfo> AppInfos { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("offlineTime")]
        public DateTime OfflineTime { get; set; }

        [JsonProperty("userStatus")]
        public string UserStatus { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("currentFamilyId")]
        public string CurrentFamilyId { get; set; }

        [JsonProperty("bindInfos")]
        public BindInfos BindInfos { get; set; }

    }
}