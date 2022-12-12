namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class SwitchParameters : Parameters
    {
        [JsonProperty("pulse")]
        public string? Pulse { get; set; }

        [JsonProperty("pulseWidth")]
        public int? PulseWidth { get; set; }

        [JsonProperty("switch")]
        public SwitchState Switch { get; set; }

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
