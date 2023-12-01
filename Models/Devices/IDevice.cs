namespace EWeLink.Api.Models.Devices
{
    using System;
    using System.Collections.Generic;
    using EWeLink.Api.Models.Converters;
    using Newtonsoft.Json;

    [JsonConverter(typeof(DeviceConverter))]
    public interface IDevice
    {
        string? Id { get; set; }

        Family? Family { get; set; }

        string? Group { get; set; }

        bool? Online { get; set; }

        List<object>? ShareUsers { get; set; }

        List<object>? Groups { get; set; }

        List<object> DevGroups { get; set; }

        string? DeviceId { get; set; }

        string? Name { get; set; }

        string? Type { get; set; }

        string? ApiKey { get; set; }

        Extra? Extra { get; set; }

        Settings? Settings { get; set; }

        DateTime CreatedAt { get; set; }

        List<object> ShareUsersInfo { get; set; }

        List<object> SharedTo { get; set; }

        int V { get; set; }

        DateTime? OnlineTime { get; set; }

        string? Ip { get; set; }

        string? Location { get; set; }

        DateTime? OfflineTime { get; set; }

        string? DeviceKey { get; set; }

        string? DeviceUrl { get; set; }

        string? BrandName { get; set; }

        string? ProductModel { get; set; }

        bool ShowBrand { get; set; }

        string BrandLogoUrl { get; set; }

        DevConfig? DeviceConfiguration { get; set; }

        int Uiid { get; set; }

        bool HasLanControl { get; }

        LanControlInformation? LanControl { get; set; }
    }

    public interface IDevice<out T, out TT> : IDevice
        where T : Parameters.Parameters
    {
        public T Parameters { get; }

        public TT Tags { get; }
    }

    public class Device<T, TT> : Device, IDevice<T, TT>
        where T : Parameters.Parameters
        where TT : Tags
    {
        [JsonProperty("params")]
        public T Parameters { get; set; } = null!;

        [JsonProperty("tags")]
        public TT? Tags { get; set; }
    }
}