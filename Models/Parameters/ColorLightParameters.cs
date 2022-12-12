namespace EWeLink.Api.Models.Parameters
{
    using EWeLink.Api.Models.LightThemes;
    using Newtonsoft.Json;

    public class ColorLightParameters : LightParameters
    {
        [JsonProperty("ltype")]
        public LightType LightType { get; set; }

        [JsonProperty("color")]
        public Color? Color { get; set; }

        [JsonProperty("read")]
        public Read? Read { get; set; }

        [JsonProperty("party")]
        public Party? Party { get; set; }

        public override Parameters CreateParameters()
        {
            var parameters = new ColorLightParameters()
            {
                LightType = this.LightType,
                Switch = this.Switch,
            };
            return parameters;
        }
    }
}
