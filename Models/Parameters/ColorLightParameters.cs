namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;

    public class ColorLightParameters : LightParameters
    {
        [JsonProperty("ltype")]
        public LightType LightType { get; set; }

        [JsonProperty("white")]
        public White White { get; set; }

        [JsonProperty("color")]
        public Color Color { get; set; }

        public override dynamic CreateParameters()
        {
            var parameters = base.CreateParameters();
            parameters.ltype = LightType;
            return parameters;
        }
    }
}
