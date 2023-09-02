namespace EWeLink.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EWeLink.Api.Models;
    using EWeLink.Api.Models.Devices;
    using EWeLink.Api.Models.EventParameters;
    using EWeLink.Api.Models.LightThemes;
    using EWeLink.Api.Models.Parameters;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using ColorLightParameters = EWeLink.Api.Models.Parameters.ColorLightParameters;

    public class Link : ILink, ILinkAuthorization
    {
        private const string AppIdValue = "4s1FXKC9FaGfoqXhmXSJneb3qcm1gOak";//"YzfeftUVcZ6twZw1OoVKPRFYTrGEg01Q";

        private const string AppSecret = "oKvCM06gvwkRbfetd6qWRrbC3rFrbIpV";//"4G91qSoboqYO4Y0XJ0LPPKIsq8reHdfa";

        private readonly ILinkConfiguration configuration;

        private readonly IDeviceCache deviceCache;

        private readonly ILinkWebSocket linkWebSocket;

        private readonly ILinkLanControl lanControl;

        private readonly IHttpClientFactory httpClientFactory;

        private string? region = "us";

        private string? email;

        private string? phoneNumber;

        private string? password;

        private string? at;

        private string? apiKey;

        private OAuhToken? auhToken;

        public Link(ILinkConfiguration configuration, IDeviceCache deviceCache, ILinkWebSocket linkWebSocket, ILinkLanControl lanControl, IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.deviceCache = deviceCache;
            this.linkWebSocket = linkWebSocket;
            this.lanControl = lanControl;
            this.httpClientFactory = httpClientFactory;
            this.lanControl.ParametersUpdated += e => LanParametersUpdated?.Invoke(e);
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
            this.auhToken = configuration.AuhToken;
            this.apiKey = configuration.ApiKey;
        }

        public event Action<ILinkEvent<IEventParameters>>? LanParametersUpdated;

        public event Action<OAuhToken>? OAuthTokenUpdated;

        public static string AppId => AppIdValue;

        public Uri ApiUri => new Uri($"https://{this.region}-apia.coolkit.cc");

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

        public Task<IDevice?> GetDevice(string deviceId) => this.GetDevice(deviceId, false);

        public int GetDeviceChannelCountByUuid(int uuid)
        {
            var deviceType = this.GetDeviceTypeByUiid(uuid);
            return DeviceData.DeviceChannelCount[deviceType];
        }

        public Task ToggleDevice(string deviceId, ChannelId channel = ChannelId.One)
        {
            return this.SetDevicePowerState(deviceId, SwitchStateChange.Toggle, channel);
        }

        public async Task<int> GetDeviceChannelCount(string deviceId)
        {
            var device = await this.GetDevice(deviceId);
            if (device.Extra is null)
            {
                throw new ArgumentNullException(nameof(device.Extra));
            }

            var uiid = device.Extra.Uiid;
            var switchesAmount = this.GetDeviceChannelCountByUuid(uiid);

            return switchesAmount;
        }

        public async Task SetDevicePowerState(string deviceId, SwitchStateChange state, ChannelId channel = ChannelId.One)
        {
            var device = await this.GetDevice(deviceId);
            if (device == null)
            {
                throw new KeyNotFoundException("The device id was not found");
            }

            if (device.Extra is null)
            {
                throw new ArgumentNullException(nameof(device.Extra));
            }

            var uiid = device.Extra.Uiid;

            var switchesAmount = this.GetDeviceChannelCountByUuid(uiid);
            if (switchesAmount > 0 && switchesAmount < (int)channel)
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
                case ITwoSwitchDevice twoSwitch:
                    {
                        status = channel switch
                        {
                            ChannelId.One => twoSwitch.Parameters.One.Switch,
                            ChannelId.Two => twoSwitch.Parameters.Two.Switch,
                            _ => throw new ArgumentOutOfRangeException(nameof(channel))
                        };
                        switches = twoSwitch.Parameters.Switches;
                    }

                    break;
                case IThreeSwitchDevice threeSwitchDevice:
                    {
                        status = channel switch
                        {
                            ChannelId.One => threeSwitchDevice.Parameters.One.Switch,
                            ChannelId.Two => threeSwitchDevice.Parameters.Two.Switch,
                            ChannelId.Three => threeSwitchDevice.Parameters.Three.Switch,
                            _ => throw new ArgumentOutOfRangeException(nameof(channel))
                        };
                        switches = threeSwitchDevice.Parameters.Switches;
                    }

                    break;

                case IFourSwitchDevice fourSwitchDevice:
                    {
                        status = channel switch
                        {
                            ChannelId.One => fourSwitchDevice.Parameters.One.Switch,
                            ChannelId.Two => fourSwitchDevice.Parameters.Two.Switch,
                            ChannelId.Three => fourSwitchDevice.Parameters.Three.Switch,
                            ChannelId.Four => fourSwitchDevice.Parameters.Four.Switch,
                            _ => throw new ArgumentOutOfRangeException(nameof(channel))
                        };
                        switches = fourSwitchDevice.Parameters.Switches;
                    }

                    break;
                default:
                    throw new NotSupportedException("Device is not a switch");
            }

            var deviceParameters = ((IDevice<Parameters>)device).Parameters;
            var parameters = deviceParameters.CreateParameters();

            var stateToSwitch = state switch
                {
                    SwitchStateChange.Toggle => status == SwitchState.On ? SwitchState.Off : SwitchState.On,
                    SwitchStateChange.On => SwitchState.On,
                    SwitchStateChange.Off => SwitchState.Off,
                    _ => throw new ArgumentOutOfRangeException(nameof(state))
                };

            if (parameters is MultiSwitchParameters multiSwitchParameters)
            {
                var linkSwitch = multiSwitchParameters.Switches.First(x => x.Outlet == (int)channel);
                linkSwitch!.Switch = stateToSwitch;

                // Reduce the switches to only include the channel we want changed
                multiSwitchParameters.Switches = new[] { linkSwitch };
            }
            else if (parameters is SwitchParameters switchParameters)
            {
                switchParameters.Switch = stateToSwitch;
            }
            else
            {
                throw new ArgumentException(nameof(parameters));
            }

            if (device.HasLanControl)
            {
                var handled = await this.lanControl.SendSwitchRequest(device, parameters);
                if (handled.HasValue)
                {
                    return;
                }
            }

            dynamic response = await this.MakeRequest(
                                   "v2/device/thing/status",
                                   body: new
                                             {
                                                 type = 1,
                                                 id = deviceId,
                                                 @params = parameters,
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

            var deviceParameters = (ColorLightParameters)colorLight.Parameters.CreateParameters();
            if (value is Color colorValue)
            {
                deviceParameters.LightType = LightType.Color;
                deviceParameters.Color = colorValue;
            }
            else if (value is White whiteValue)
            {
                deviceParameters.LightType = LightType.White;
                deviceParameters.White = whiteValue;
            }

            dynamic response = await this.MakeRequest(
                "v2/device/thing/status",
                body: new
                {
                    type = 1,
                    id = deviceId,
                    @params = deviceParameters,
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

        public void EnableLanControl()
        {
            this.lanControl.Start();
        }

        public async Task<List<IDevice>> GetDevices()
        {
            dynamic response = await this.MakeRequest(
                                   "v2/device/thing",
                                   query: new
                                              {
                                                  lang = "en",
                                                  num = 0,
                                              });

            JToken jtoken = response.data.thingList;
            if (jtoken == null)
            {
                throw new HttpRequestException(NiceError.Custom[CustomErrors.NoDevices]);
            }

            var devicelist = jtoken.ToObject<List<Thing>>()?.Select(x => x.ItemData).ToList();
            if (devicelist != null)
            {
                this.deviceCache.UpdateCache(devicelist);
            }

            return devicelist ?? new List<IDevice>();
        }

        public async Task<List<UpdateCheckResult>> CheckAllDeviceUpdates()
        {
            var devices = await this.GetDevices();
            var result = await this.CheckDeviceUpdates(devices);

            var genericDevices = devices.Cast<Device<Parameters>>().Where(x => x.Parameters is LinkParameters).Cast<Device<LinkParameters>>().ToDictionary(x => x.DeviceId);

            return result.Select(r => new UpdateCheckResult(r.Version != genericDevices[r.DeviceId].Parameters.FirmWareVersion, r))
                .ToList();
        }

        public async Task<List<UpgradeInfo>> CheckDeviceUpdates(IEnumerable<IDevice> devices)
        {
            var genericDevices = devices.Cast<Device<Parameters>>().Where(x => x.Parameters is LinkParameters).Cast<Device<LinkParameters>>();
            var deviceInfoList = GetFirmwareUpdateInfo(genericDevices);

            dynamic response = await this.MakeRequest(
                                   "v2/device/ota/query",
                                   body: new { deviceInfoList = deviceInfoList },
                                   method: HttpMethod.Post);

            JToken token = response.otaInfoList;
            return token.ToObject<List<UpgradeInfo>>() ?? new List<UpgradeInfo>();
        }

        public async Task<UpdateCheckResult> CheckDeviceUpdate(string deviceId)
        {
            var device = await this.GetDevice(deviceId);

            var genericDevice = device as Device<LinkParameters>;

            var result = await this.CheckDeviceUpdates(new[] { device });

            var info = result.FirstOrDefault();
            return new UpdateCheckResult(info!.Version != genericDevice!.Parameters.FirmWareVersion, info);
        }

        public async Task<string?> GetFirmwareVersion(string deviceId)
        {
            var device = await this.GetDevice(deviceId);
            var genericDevice = device as Device<LinkParameters>;
            return genericDevice!.Parameters.FirmWareVersion;
        }

        public async Task<Credentials> GetCredentials()
        {
            var body = new
            {
                password,
                countryCode = "+86",
                this.email,
            };
            var data = JsonConvert.SerializeObject(body);

            var uri = new Uri($"{this.ApiUri}v2/user/login");
            var httpMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            httpMessage.Headers.Add("Authorization", $"Sign {this.MakeAuthorizationSign(data)}");
            httpMessage.Headers.Add("X-CK-Appid", AppId);
            httpMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpMessage);

            response.EnsureSuccessStatusCode();
            var jsonString = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject<dynamic>(jsonString);

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

            JToken token = json.data;
            var credentials = token.ToObject<Credentials>() ?? new Credentials();
            this.apiKey = credentials.User?.Apikey;
            this.at = credentials.At;
            return credentials;
        }

        public (string Signature, long Seq) GetAuthorization()
        {
            var seq = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var authorization = MakeAuthorizationSign($"{AppId}_{seq}");
            return (authorization, seq);
        }

        public async Task<OAuhToken> GetAccessToken(string code, string redirectUri)
        {
            var body = new
            {
                code,
                redirectUrl = redirectUri,
                grantType = "authorization_code",
            };

            var uri = new Uri($"{this.ApiUri}v2/user/oauth/token");
            var httpMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var data = JsonConvert.SerializeObject(body, new StringEnumConverter());
            httpMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpMessage);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<OAuhToken>(json)!;
        }

        public async Task ReloadAccessToken()
        {
            this.auhToken = this.configuration.AuhToken;
            if (this.auhToken is null)
            {
                return;
            }

            await ValidateAuthToken();
        }

        private async Task ValidateAuthToken()
        {
            if (this.auhToken is null)
            {
                throw new ArgumentNullException(nameof(auhToken), "ValidateAuthToken called without a access token");
            }

            var accessExpiry = DateTimeOffset.FromUnixTimeMilliseconds(this.auhToken.AccessTokenExpiredTime);
            if (accessExpiry - TimeSpan.FromDays(2) > DateTimeOffset.Now)
            {
                this.at = this.auhToken.AccessToken;
                return;
            }

            var refreshExpiry = DateTimeOffset.FromUnixTimeMilliseconds(this.auhToken.RefreshTokenExpiredTime);
            if (refreshExpiry < DateTimeOffset.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(auhToken.RefreshToken), "Refresh Token expired");
            }

            var body = new { rt = auhToken.RefreshToken, };
            var uri = new Uri($"{this.ApiUri}v2/user/refresh");
            var httpMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var data = JsonConvert.SerializeObject(body, new StringEnumConverter());
            httpMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");

            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpMessage);
            var refreshTime = DateTimeOffset.Now;
            response.EnsureSuccessStatusCode();
            var rawJson = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<dynamic>(rawJson)!;

            string accessToken = json.at;
            long accessTokenExpiredTime = refreshTime.AddDays(30).ToUnixTimeMilliseconds();
            string refreshToken = json.rt;
            long refreshTokenExpiredTime = refreshTime.AddDays(60).ToUnixTimeMilliseconds();

            var authToken = new OAuhToken
            {
                AccessToken = accessToken, AccessTokenExpiredTime = accessTokenExpiredTime,
                RefreshToken = refreshToken,
                RefreshTokenExpiredTime = refreshTokenExpiredTime,
            };
            this.auhToken = authToken;
            this.at = auhToken.AccessToken;
            this.OAuthTokenUpdated?.Invoke(authToken);

            // Get api key from the family
            dynamic homeList = MakeRequest("v2/family", query: new { lang = "en" });
            dynamic[] list = homeList.familyList;
            string currentId = homeList.currentFamilyId;
            dynamic item = list.First(x => x.id == currentId);
            this.apiKey = item.apiKey;
        }

        private async Task<dynamic> MakeRequest(string path, Uri? baseUri = null, object? body = null, object? query = null, HttpMethod? method = null)
        {
            if (method == null)
            {
                method = HttpMethod.Get;
            }

            if (this.auhToken != null)
            {
                await ValidateAuthToken();
            }

            if (string.IsNullOrWhiteSpace(this.at) && this.auhToken is null)
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
            httpMessage.Headers.Add("X-CK-Appid", AppId);

            if (body != null)
            {
                var data = JsonConvert.SerializeObject(body, new StringEnumConverter());
                httpMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }

            var httpClient = this.httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpMessage);

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

        public async Task<IDevice?> GetDevice(string deviceId, bool noCacheLoad)
        {
            if (!noCacheLoad && this.deviceCache.TryGetDevice(deviceId, out IDevice? device))
            {
                return device!;
            }

            dynamic response = await this.MakeRequest(
                $"v2/device/thing",
                body: new
                {
                    thingList = new[]
                    {
                        new
                        {
                            itemType = 2,
                            id = deviceId,
                        },
                    },
                }, method: HttpMethod.Post);

            JToken token = response.data.thingList;
            if (token == null)
            {
                throw new KeyNotFoundException("The device id was not found");
            }

            device = token.ToObject<List<Thing>>()?.Select(x => x.ItemData).FirstOrDefault();
            if (device != null)
            {
                return this.deviceCache.UpdateCache(device);
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

        private string MakeAuthorizationSign(string data)
        {
            var crypto = HMACSHA256.Create("HmacSHA256");
            crypto.Key = Encoding.UTF8.GetBytes(AppSecret);
            var hash = crypto.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hash);
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

        private List<FirmwareUpdateInfo> GetFirmwareUpdateInfo(IEnumerable<Device<LinkParameters>> devices)
        {
            return devices.Select(
                    d => new FirmwareUpdateInfo
                    {
                        Model = d.Extra?.Model,
                        Version = d.Parameters.Version,
                        DeviceId = d.DeviceId,
                    })
                .ToList();
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
            public string? AppId { get; }

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
