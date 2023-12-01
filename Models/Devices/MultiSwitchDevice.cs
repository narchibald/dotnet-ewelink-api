namespace EWeLink.Api.Models.Devices
{
    using EWeLink.Api.Models.Parameters;
    using Newtonsoft.Json;

    public abstract class MultiSwitchDevice<T> : Device<T>
        where T : MultiSwitchParameters
    {
        [JsonProperty("tags")]
        public new SwitchTags? Tags
        {
            get => base.Tags as SwitchTags;
            set => base.Tags = value;
        }
    }
}