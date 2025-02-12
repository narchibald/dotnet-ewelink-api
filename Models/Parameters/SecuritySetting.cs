namespace EWeLink.Api.Models.Parameters;

using System.Collections.Generic;
using Newtonsoft.Json;

public class SecuritySetting
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("triggers")]
    public List<string> Triggers { get; set; }

    [JsonProperty("operation")]
    public SecurityOperation Operation { get; set; }
}