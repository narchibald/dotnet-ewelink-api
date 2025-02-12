namespace EWeLink.Api.Models.Parameters;

using Newtonsoft.Json;

public class SecurityOperation
{
    [JsonProperty("buzzerAlarm")]
    public BuzzerAlarm BuzzerAlarm { get; set; }
}