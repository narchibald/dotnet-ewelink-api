namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Parameters
    {
        [JsonProperty("bindInfos")]
        public BindInfos? BindInfos { get; set; }

        public virtual Parameters CreateParameters() => new Parameters();

        public int? Update(dynamic data) => Update(JsonConvert.SerializeObject(data, new StringEnumConverter()));

        public virtual int? Update(string jsonData)
        {
            JsonConvert.PopulateObject(jsonData, this);
            return null;
        }
    }
}