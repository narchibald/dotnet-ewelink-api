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

        public override dynamic CreateParameters()
        {
            var parameters = base.CreateParameters();
            parameters.ltype = LightType;
            return parameters;
        }
    }
}
