namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class ZbMicroParameters : SnZbParameters
    {
        [JsonProperty("switch")]
        public SwitchState Switch { get; set; }

        [JsonProperty("startup")]
        public StartupState Startup { get; set; }

        [JsonProperty("wallPenetration")]
        public bool WallPenetration { get; set; }

        public override Parameters CreateParameters()
        {
            var parameters = new SwitchParameters()
            {
                Switch = this.Switch
            };
            return parameters;
        }
    }
}