using EWeLink.Api.Models.LightThemes;
using Newtonsoft.Json.Converters;

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
    using EWeLink.Api.Models.Devices;
    using EWeLink.Api.Models.Parameters;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Link : ILink
    {
        private const string AppId = "YzfeftUVcZ6twZw1OoVKPRFYTrGEg01Q";

        private const string AppSecret = "4G91qSoboqYO4Y0XJ0LPPKIsq8reHdfa";

        private static readonly HttpClient HttpClient = new ();

        private readonly IDeviceCache deviceCache;

        private readonly ILinkWebSocket linkWebSocket;

        private string? region = "us";

        private string? email;

        private string? phoneNumber;

        private string? password;

        private string? at;

        private string? apiKey;

        public Link(ILinkConfiguration configuration, IDeviceCache deviceCache, ILinkWebSocket linkWebSocket)
        {
            this.deviceCache = deviceCache;
            this.linkWebSocket = linkWebSocket;
            var check = this.CheckLoginParameters(configuration.Email, configuration.PhoneNumber, configuration.Password, configuration.At);

            if (!check)
            {
                throw new Exception("invalidCredentials");
            }

            this.region = configuration.Region;
            this.phoneNumber = configuration.PhoneNumber;
            this.email = configuration.Email;
            this.password = configuration.Password;
            this.at = configuration.At;
            this.apiKey = configuration.ApiKey;
        }

        public Uri ApiUri => new Uri($"https://{this.region}-api.coolkit.cc:8080/api");

        public Uri OtaUri => new Uri($"https://{this.region}-ota.coolkit.cc:8080/otaother");

        public Uri ApiWebSocketUri => new Uri($"wss://{this.region}-pconnect3.coolkit.cc:8080/api/ws");

        public async Task<SensorData?> GetDeviceCurrentSensorData(string deviceId)
        {
            var device = await this.GetDevice(deviceId, true);

            if (device is IThermostatDevice thermostatDevice)
            {
                var parameters = thermostatDevice.Parameters;
                if (!parameters.Temperature.HasValue && !parameters.Humidity.HasValue)
                {
                    return null;
                }

                if (parameters.Temperature.HasValue)
                {
                    return new SensorData(deviceId, SensorType.Temperature, parameters.Temperature.Value);
                }

                if (parameters.Humidity.HasValue)
                {
                    return new SensorData(deviceId, SensorType.Humidity, parameters.Humidity.Value);
                }
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
            return this.SetDevicePowerState(deviceId, SwitchStateChange.Toggle, channel);
        }

        public async Task<int> GetDeviceChannelCount(string deviceId)
        {
            var device = await this.GetDevice(deviceId);
            var uiid = device.Extra.Extended.Uiid;
            var switchesAmount = this.GetDeviceChannelCountByUuid(uiid);

            return switchesAmount;
        }

        public async Task SetDevicePowerState(string deviceId, SwitchStateChange state, int channel = 1)
        {
            var device = await this.GetDevice(deviceId);

            if (device == null)
            {
                throw new KeyNotFoundException("The device id was not found");
            }

            var uiid = device.Extra.Extended.Uiid;

            var switchesAmount = this.GetDeviceChannelCountByUuid(uiid);
            if (switchesAmount > 0 && switchesAmount < channel)
            {
                throw new Exception(NiceError.Custom[CustomErrors.Ch404]);
            }

            var switches = new LinkSwitch[switchesAmount];
            SwitchState status;
            switch (device)
            {
                case ISingleSwitchDevice singleSwitch:
                    status = singleSwitch.Parameters.Switch;
                    break;
                case IMultiSwitchDevice multiSwitch:
                    status = multiSwitch.Parameters.Switches[channel - 1].Switch;
                    switches = multiSwitch.Parameters.Switches;
                    break;
                default:
                    throw new NotSupportedException("Device is not a switch");
            }

            var deviceParameters = ((IDevice<Paramaters>)device).Parameters;
            dynamic parameters = deviceParameters.CreateParameters();

            var stateToSwitch = state switch
                {
                    SwitchStateChange.Toggle => status == SwitchState.On ? SwitchState.Off : SwitchState.On,
                    SwitchStateChange.On => SwitchState.On,
                    SwitchStateChange.Off => SwitchState.Off,
                    _ => throw new ArgumentOutOfRangeException(nameof(state))
                };

            if (switches.Length > 1)
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
            else
            {
                deviceParameters.Update(parameters);
            }
        }

        public async Task SetLightColor(string deviceId, LightBrightness value)
        {
            var device = await this.GetDevice(deviceId);

            if (device == null)
            {
                throw new KeyNotFoundException("The device id was not found");
            }

            if (!(device is Device<ColorLightParameters> colorLight))
            {
                throw new NotSupportedException("The given device id is not a color light");
            }

            var deviceParameters = colorLight.Parameters.CreateParameters();
            if (value is Color colorValue)
            {
                deviceParameters.ltype = LightType.Color;
                deviceParameters.color = colorValue;
            }
            else if (value is White whiteValue)
            {
                deviceParameters.ltype = LightType.White;
                deviceParameters.white = whiteValue;
            }

            dynamic response = await this.MakeRequest(
                "/user/device/status",
                body: new
                {
                    deviceid = deviceId,
                    appid = AppId,
                    @params = deviceParameters,
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
            else
            {
                colorLight.Parameters.Update(deviceParameters);
            }
        }

        public async Task<ILinkWebSocket> OpenWebSocket(CancellationToken cancellationToken = default)
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

            await this.linkWebSocket.Open(wssLoginPayload, ApiWebSocketUri, cancellationToken);
            return this.linkWebSocket;
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
            this.deviceCache.UpdateCache(devicelist);
            return devicelist;
        }

        public async Task<List<UpdateCheckResult>> CheckAllDeviceUpdates()
        {
            var devices = await this.GetDevices();
            var result = await this.CheckDeviceUpdates(devices);

            var genericDevices = devices.Cast<Device<Paramaters>>().Where(x => x.Parameters is LinkParamaters).Cast<Device<LinkParamaters>>().ToDictionary(x => x.Deviceid);

            return result.Select(r => new UpdateCheckResult(r.Version != genericDevices[r.DeviceId].Parameters.FirmWareVersion, r))
                .ToList();
        }

        public async Task<List<UpgradeInfo>> CheckDeviceUpdates(IEnumerable<Device> devices)
        {
            var genericDevices = devices.Cast<Device<Paramaters>>().Where(x => x.Parameters is LinkParamaters).Cast<Device<LinkParamaters>>();
            var deviceInfoList = this.GetFirmwareUpdateInfo(genericDevices);

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

            var genericDevice = device as Device<LinkParamaters>;

            var result = await this.CheckDeviceUpdates(new[] { device });

            var info = result.FirstOrDefault();
            return new UpdateCheckResult(info.Version != genericDevice.Parameters.FirmWareVersion, info);
        }

        public async Task<string?> GetFirmwareVersion(string deviceId)
        {
            var device = await this.GetDevice(deviceId);
            var genericDevice = device as Device<LinkParamaters>;
            return genericDevice.Parameters.FirmWareVersion;
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
                var data = JsonConvert.SerializeObject(body, new StringEnumConverter());
                httpMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }

            var response = await HttpClient.SendAsync(httpMessage);

            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(jsonString);

            int? error = json.error;

            if (error > 0)
            {
                if (error == 406)
                {
                    this.at = null;
                }

                throw new HttpRequestException(NiceError.Errors[error.Value]);
            }

            return json;
        }

        private async Task<Device> GetDevice(string deviceId, bool noCacheLoad)
        {
            if (!noCacheLoad && this.deviceCache.TryGetDevice(deviceId, out Device device))
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
            if (token == null)
            {
                throw new KeyNotFoundException("The device id was not found");
            }

            device = token.ToObject<Device>();

            return this.deviceCache.UpdateCache(device);
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

        private List<FirmwareUpdateInfo> GetFirmwareUpdateInfo(IEnumerable<Device<LinkParamaters>> devices)
        {
            return devices.Select(d => new FirmwareUpdateInfo
                                           {
                                               Model = d.Extra.Extended.Model,
                                               Version = d.Parameters.Version,
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
