namespace EWeLink.Api.Models.EventParameters
{
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;

    public interface ISnZbMotionPEventParameters
        : ISnZbMotionEventParameters
    {
        IlluminationLevel? BrState { get; set; }
    }

    [EventDeviceIdentifier(7002)]
    public class SnZbMotionPEventParameters
        : SnZbMotionEventParameters, ISnZbMotionPEventParameters
    {
        [JsonProperty("brState")]
        public IlluminationLevel? BrState { get; set; }
    }
}