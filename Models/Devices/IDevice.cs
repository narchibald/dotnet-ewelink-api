namespace EWeLink.Api.Models.Devices
{
    using Newtonsoft.Json;

    public interface IDevice<out T>
        where T : Parameters.Parameters
    {
        public T Parameters { get; }
    }

    public class Device<T> : Device, IDevice<T>
        where T : Parameters.Parameters
    {
        [JsonProperty("params")]
        public T Parameters { get; set; } = null!;
    }
}