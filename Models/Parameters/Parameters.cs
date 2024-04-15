namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Parameters
    {
        [JsonProperty("bindInfos")]
        public BindInfos? BindInfos { get; set; }

        [JsonProperty("fwVersion")]
        public string FirmwareVersion { get; set; } = string.Empty;

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        public virtual Parameters CreateParameters() => new Parameters();

        public int? Update(dynamic data) => Update(JsonConvert.SerializeObject(data, new StringEnumConverter()));

        public virtual int? Update(string jsonData)
        {
            JsonConvert.PopulateObject(jsonData, this);
            return null;
        }
    }
}