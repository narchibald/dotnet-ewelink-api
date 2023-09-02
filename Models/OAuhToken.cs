namespace EWeLink.Api.Models
{
    using Newtonsoft.Json;

    public class OAuhToken
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonProperty("atExpiredTime")]
        public long AccessTokenExpiredTime { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonProperty("rtExpiredTime")]
        public long RefreshTokenExpiredTime { get; set; }
    }
}