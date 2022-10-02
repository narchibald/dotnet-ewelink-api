namespace EWeLink.Api.Models.Parameters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class Parameters
    {
        [JsonProperty("bindInfos")]
        public BindInfos BindInfos { get; set; }

        public virtual dynamic CreateParameters() => new System.Dynamic.ExpandoObject();

        public void Update(dynamic data) => Update(JsonConvert.SerializeObject(data, new StringEnumConverter()));

        public void Update(string jsonData) => JsonConvert.PopulateObject(jsonData, this);
    }
}