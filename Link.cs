namespace EWeLink.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.WebSockets;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using EWeLink.Api.Models;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Link
    {
        private const string AppId = "YzfeftUVcZ6twZw1OoVKPRFYTrGEg01Q";

        private const string AppSecret = "4G91qSoboqYO4Y0XJ0LPPKIsq8reHdfa";

        private static readonly HttpClient HttpClient = new ();

        private readonly Dictionary<string, Device> devicesCache = new ();

        private string? region = "us";

        private string? email;

        private string? phoneNumber;

        private string? password;

        private string? at;

        private string? apiKey;

        public Link(
            string? email = null,
            string? password = null,
            string? phoneNumber = null,
            string? region = "us",
            string? at = null,
            string? apiKey = null)
        {
            var check = this.CheckLoginParameters(email, phoneNumber, password, at);

            if (!check)
            {
                throw new Exception("invalidCredentials");
            }

            this.region = region;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.password = password;
            this.at = at;
            this.apiKey = apiKey;
        }

        public Uri ApiUri => new Uri($"https://{this.region}-api.coolkit.cc:8080/api");

        public Uri OtaUri => new Uri($"https://{this.region}-ota.coolkit.cc:8080/otaother");

        public Uri ApiWebSocketUri => new Uri($"wss://{this.region}-pconnect3.coolkit.cc:8080/api/ws");

        public async Task<SensorData?> GetDeviceCurrentSensorData(string deviceId)
        {
            var device = await this.GetDevice(deviceId, true);

            var parameters = device.Paramaters;
            if (!parameters.CurrentTemperature.HasValue && !parameters.CurrentHumidity.HasValue)
            {
                return null;
            }

            if (parameters.CurrentTemperature.HasValue)
            {
                return new SensorData(deviceId, SensorType.Temperature, parameters.CurrentTemperature.Value);
            }

            if (parameters.CurrentHumidity.HasValue)
            {
                return new SensorData(deviceId, SensorType.Humidity, parameters.CurrentHumidity.Value);
            }

            return null;
        }

        public async Task<(string? Email, string? Region)> GetRegion()
        {
            if (string.IsNullOrWhiteSpace(this.email))
            {
                throw new ArgumentNullException(nameof(this.email));
            }

            if (string.IsNullOrWhiteSpace(this.password))
            {
                throw new ArgumentNullException(nameof(this.password));
            }

            var credentials = await this.GetCredentials();

            return (credentials.User?.Email, credentials.Region);
        }

        public Task<Device> GetDevice(string deviceId)
        {
            return this.GetDevice(deviceId, false);
        }

        public int GetDeviceChannelCountByUuid(int uuid)
        {
            var deviceType = this.GetDeviceTypeByUiid(uuid);
            return DeviceData.DeviceChannelCount[deviceType];
        }

        public Task ToggleDevice(string deviceId, int channel = 1)
        {
            return this.SetDevicePowerState(deviceId, "toggle", channel);
        }

        public async Task<int> GetDeviceChannelCount(string deviceId)
        {
            var device = await this.GetDevice(deviceId);
            var uiid = device.Extra.Extended.Uiid;
            var switchesAmount = this.GetDeviceChannelCountByUuid(uiid);

            return switchesAmount;
        }

        public async Task SetDevicePowerState(string deviceId, string state, int channel = 1)
        {
            var device = await this.GetDevice(deviceId);
            var uiid = device.Extra.Extended.Uiid;

            var status = device.Paramaters.Switch;
            var switches = device.Paramaters.Switches;

            var switchesAmount = this.GetDeviceChannelCountByUuid(uiid);

            if (switchesAmount > 0 && switchesAmount < channel)
            {
                throw new Exception(NiceError.Custom[CustomErrors.Ch404]);
            }

            var stateToSwitch = state;
            dynamic parameters = new System.Dynamic.ExpandoObject();

            if (switches != null)
            {
                status = switches[channel - 1].Switch;
            }

            if (state == "toggle")
            {
                stateToSwitch = status == SwitchState.On ? "off" : "on";
            }

            if (switches != null)
            {
                parameters.switches = switches;
                parameters.switches[channel - 1].@switch = stateToSwitch;
            }
            else
            {
                parameters.@switch = stateToSwitch;
            }

            dynamic response = await this.MakeRequest(
                                   "/user/device/status",
                                   body: new
                                             {
                                                 deviceid = deviceId,
                                                 appid = AppId,
                                                 @params = parameters,
                                                 nonce = Utilities.Nonce,
                                                 ts = Utilities.Timestamp,
                                                 version = 8,
                                             },
                                   method: HttpMethod.Post);

            int? responseError = response.error;

            if (responseError > 0)
            {
                throw new Exception(NiceError.Errors[responseError.Value]);
            }
        }

        public Task<ClientWebSocket> OpenWebSocket()
        {
            return this.OpenWebSocket(CancellationToken.None);
        }

        public async Task<ClientWebSocket> OpenWebSocket(CancellationToken cancellationToken)
        {
            var wssLoginPayload = JsonConvert.SerializeObject(new
            {
                action = "userOnline",
                at = this.at,
                apikey = this.apiKey,
                appid = AppId,
                nonce = Utilities.Nonce,
                ts = Utilities.Timestamp,
                userAgent = "ewelink-api",
                sequence = 1,
                version = 8,
            });

            var wsp = new ClientWebSocket();
            wsp.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(120000);

            await wsp.ConnectAsync(this.ApiWebSocketUri, CancellationToken.None);

            var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(wssLoginPayload));
            await wsp.SendAsync(data, WebSocketMessageType.Text, true, cancellationToken);

            var buffer = new ArraySegment<byte>(new byte[8192]);
            var result = await wsp.ReceiveAsync(buffer, cancellationToken);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var text = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                var json = JsonConvert.DeserializeObject<dynamic>(text);
                int? error = json.error;
                if (error > 0)
                {
                    throw new Exception($"Error: {error}");
                }
            }

            return wsp;
        }

        public async Task<List<Device>> GetDevices()
        {
            dynamic response = await this.MakeRequest(
                                   "/user/device",
                                   query: new
                                              {
                                                  lang = "en",
                                                  appid = AppId,
                                                  ts = Utilities.Timestamp,
                                                  version = 8,
                                                  getTags = 1,
                                              });

            JToken jtoken = response.devicelist;
            if (jtoken == null)
            {
                throw new HttpRequestException(NiceError.Custom[CustomErrors.NoDevices]);
            }

            var devicelist = jtoken.ToObject<List<Device>>();
            foreach (var device in devicelist)
            {
                if (!this.devicesCache.ContainsKey(device.Deviceid))
                {
                    this.devicesCache.Add(device.Deviceid, device);
                }
                else
                {
                    this.devicesCache[device.Deviceid] = device;
                }
            }

            return devicelist;
        }

        public async Task<List<UpdateCheckResult>> CheckAllDeviceUpdates()
        {
            var devices = await this.GetDevices();
            var result = await this.CheckDeviceUpdates(devices);

            return result.Select(r => new UpdateCheckResult(r.Version != devices.First(d => d.Deviceid == r.DeviceId).Paramaters.FirmWareVersion, r))
                .ToList();
        }

        public async Task<List<UpgradeInfo>> CheckDeviceUpdates(IEnumerable<Device> devices)
        {
            var deviceInfoList = this.GetFirmwareUpdateInfo(devices);

            dynamic response = await this.MakeRequest(
                                   "/app",
                                   this.OtaUri,
                                   new { deviceInfoList = deviceInfoList },
                                   method: HttpMethod.Post);

            int returnCode = response.rtnCode;
            JToken token = response.upgradeInfoList;
            return token.ToObject<List<UpgradeInfo>>();
        }

        public async Task<UpdateCheckResult> CheckDeviceUpdate(string deviceId)
        {
            var device = await this.GetDevice(deviceId);

            var result = await this.CheckDeviceUpdates(new[] { device });

            var info = result.FirstOrDefault();
            return new UpdateCheckResult(info.Version != device.Paramaters.FirmWareVersion, info);
        }

        public async Task<string?> GetFirmwareVersion(string deviceId)
        {
            var device = await this.GetDevice(deviceId);
            return device.Paramaters.FirmWareVersion;
        }

        public async Task<Credentials> GetCredentials()
        {
            var body = this.CredentialsPayload(email: this.email, phoneNumber: this.phoneNumber, password: this.password);

            var uri = new Uri($"{this.ApiUri}/user/login");
            var httpMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            httpMessage.Headers.Add("Authorization", $"Sign {this.MakeAuthorizationSign(body)}");
            httpMessage.Content = new StringContent(JsonConvert.SerializeObject(body));

            var response = await HttpClient.SendAsync(httpMessage);

            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(jsonString);

            int? errorValue = json.error;
            string region = json.region;

            if (errorValue.HasValue && new[] { 400, 401, 404 }.Contains(errorValue.Value))
            {
                throw new HttpRequestException(NiceError.Errors[406]);
            }

            if (errorValue == 301 && region != null)
            {
                if (this.region != region)
                {
                    this.region = region;
                    return await this.GetCredentials();
                }

                throw new ArgumentOutOfRangeException(nameof(this.region), "Region does not exist");
            }

            JToken token = json;
            var credentials = token.ToObject<Credentials>();
            this.apiKey = credentials.User.Apikey;
            this.at = credentials.At;
            return credentials;
        }

        private async Task<dynamic> MakeRequest(string path, Uri? baseUri = null, object? body = null, object? query = null, HttpMethod? method = null)
        {
            if (method == null)
            {
                method = HttpMethod.Get;
            }

            if (string.IsNullOrWhiteSpace(this.at))
            {
                await this.GetCredentials();
            }

            if (baseUri == null)
            {
                baseUri = this.ApiUri;
            }

            var uriBuilder = new UriBuilder($"{baseUri}{path}")
            {
                Query = query != null ? this.ToQueryString(query) : string.Empty,
            };

            var uri = uriBuilder.Uri;
            var httpMessage = new HttpRequestMessage(method, uri);
            httpMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.at);

            if (body != null)
            {
                var data = JsonConvert.SerializeObject(body);
                httpMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }

            var response = await HttpClient.SendAsync(httpMessage);

            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(jsonString);

            int? error = json.error;

            if (error > 0)
            {
                throw new HttpRequestException(NiceError.Errors[error.Value]);
            }

            return json;
        }

        private async Task<Device> GetDevice(string deviceId, bool noCacheLoad)
        {
            if (!noCacheLoad && this.devicesCache.TryGetValue(deviceId, out Device device))
            {
                return device;
            }

            dynamic response = await this.MakeRequest(
                                   $"/user/device/{deviceId}",
                                   query: new
                                              {
                                                  deviceid = deviceId,
                                                  appid = AppId,
                                                  nonce = Utilities.Nonce,
                                                  ts = Utilities.Timestamp,
                                                  version = 8,
                                              });

            JToken token = response;

            device = token.ToObject<Device>();
            if (this.devicesCache.ContainsKey(deviceId))
            {
                this.devicesCache[deviceId] = device;
            }
            else
            {
                this.devicesCache.Add(deviceId, device);
            }

            return device;
        }

        private bool CheckLoginParameters(
            string? email,
            string? phoneNumber,
            string? password,
            string? at)
        {
            if (email != null && phoneNumber != null)
            {
                return false;
            }

            if ((email != null && password != null) || (phoneNumber != null && password != null) || at != null)
            {
                return true;
            }

            return false;
        }

        private string ToQueryString(object? qs)
        {
            var properties = from p in qs.GetType().GetProperties()
                where p.GetValue(qs, null) != null
                select p.Name + "=" + System.Web.HttpUtility.UrlEncode(p.GetValue(qs, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        private string MakeAuthorizationSign(PayLoad? body)
        {
            var crypto = HMACSHA256.Create("HmacSHA256");
            crypto.Key = Encoding.UTF8.GetBytes(AppSecret);
            var hash = crypto.ComputeHash(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body)));
            return Convert.ToBase64String(hash);
        }

        private PayLoad CredentialsPayload(string? email, string? phoneNumber, string? password)
        {
            return new PayLoad(
                AppId,
                email,
                phoneNumber,
                password,
                Utilities.Timestamp,
                8,
                Utilities.Nonce);
        }

        private string GetDeviceTypeByUiid(int uiid)
        {
            if (DeviceData.DeviceTypeUuid.TryGetValue(uiid, out var type))
            {
                return type;
            }

            return string.Empty;
        }

        private List<FirmwareUpdateInfo> GetFirmwareUpdateInfo(IEnumerable<Device> devices)
        {
            return devices.Select(d => new FirmwareUpdateInfo
                                           {
                                               Model = d.Extra.Extended.Model,
                                               Version = d.Paramaters.Version,
                                               DeviceId = d.Deviceid,
                                           }).ToList();
        }

        private class PayLoad
        {
            public PayLoad(string appId, string? email, string? phoneNumber, string? password, long timestamp, int version, string nonce)
            {
                this.AppId = appId;
                this.Email = email;
                this.PhoneNumber = phoneNumber;
                this.Password = password;
                this.Timestamp = timestamp;
                this.Version = version;
                this.Nonce = nonce;
            }

            [JsonProperty("appid")]
            public string AppId { get; }

            [JsonProperty("email")]
            public string? Email { get; }

            [JsonProperty("phoneNumber")]
            public string? PhoneNumber { get; }

            [JsonProperty("password")]
            public string? Password { get; }

            [JsonProperty("ts")]
            public long Timestamp { get; }

            [JsonProperty("version")]
            public int Version { get; }

            [JsonProperty("nonce")]
            public string Nonce { get; }
        }

        private class FirmwareUpdateInfo
        {
            [JsonProperty("model")]
            public string? Model { get; set; }

            [JsonProperty("version")]
            public string? Version { get; set; }

            [JsonProperty("deviceid")]
            public string? DeviceId { get; set; }
        }
    }
}
