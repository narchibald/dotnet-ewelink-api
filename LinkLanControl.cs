namespace EWeLink.Api
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using EWeLink.Api.Models;
    using EWeLink.Api.Models.Devices;
    using EWeLink.Api.Models.EventParameters;
    using EWeLink.Api.Models.Parameters;
    using Makaretu.Dns;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public interface ILinkLanControl
    {
        event Action<ILinkEvent<IEventParameters>>? ParametersUpdated;

        Task<bool?> SendSwitchRequest(IDevice device, Parameters data);

        void Start();

        void Stop();
    }

    public class LinkLanControl : ILinkLanControl, IDisposable
    {
        private static readonly Random Rand = new ();

        private readonly IDeviceCache deviceCache;

        private readonly ILogger<LinkLanControl> logger;

        private readonly IHttpClientFactory httpClientFactory;

        private readonly MulticastService multicastService = new ();

        private readonly ConcurrentDictionary<string, long> deviceLastSeq = new ();

        private readonly SemaphoreSlim answerLock = new (1, 1);

        private readonly JsonSerializerSettings serializerSettings = new ()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>()
            {
                new StringEnumConverter(),
            },
            NullValueHandling = NullValueHandling.Ignore,
        };

        private bool started;

        public LinkLanControl(IDeviceCache deviceCache, ILogger<LinkLanControl> logger, IHttpClientFactory httpClientFactory)
        {
            this.deviceCache = deviceCache;
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            multicastService.NetworkInterfaceDiscovered += (_, _) => this.multicastService.SendQuery("_ewelink._tcp");
            multicastService.AnswerReceived += OnAnswerReceived;
        }

        public event Action<ILinkEvent<IEventParameters>>? ParametersUpdated;

        private string Sequence => DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

        public void Start()
        {
            if (this.started)
            {
                return;
            }

            this.multicastService.Start();
            this.started = true;
        }

        public void Stop()
        {
            if (this.started)
            {
                this.multicastService.Stop();
            }
        }

        public void Dispose()
        {
            multicastService.Dispose();
            answerLock.Dispose();
        }

        public async Task<bool?> SendSwitchRequest(IDevice device, Parameters data)
        {
            var lanInformation = device.LanControl;
            if (lanInformation == null)
            {
                return null;
            }

            if (device.DeviceKey is null)
            {
                throw new ArgumentNullException(nameof(device.DeviceKey));
            }

            object jsonData = data;
            var json = JsonConvert.SerializeObject(jsonData, serializerSettings);
            var ePayload = lanInformation.EncryptionEnabled switch
            {
                true => Encrypt(device.DeviceKey, json),
                _ => new PayLoad(false, null, json)
            };

            var payload =
                new
                {
                    sequence = Sequence,
                    deviceid = device.DeviceId,
                    selfApikey = device.ApiKey,
                    ePayload.iv,
                    ePayload.encrypt,
                    ePayload.data,
                };

            var uri = FormatSwitchUrl(lanInformation.IpAddress, lanInformation.Port, data is MultiSwitchParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Headers =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") },
                    CacheControl = new () { NoStore = true },
                    Connection = { "Keep-Alive" },
                },
                Content = new StringContent(JsonConvert.SerializeObject(payload, serializerSettings), Encoding.UTF8, "application/json"),
            };

            try
            {
                var httpClient = this.httpClientFactory.CreateClient("eWeLink-lan");
                var response = await httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    var jsonRaw = await response.Content.ReadAsStringAsync();
                    var switchResponse = JsonConvert.DeserializeObject<SwitchResponse>(jsonRaw);
                    var errorCode = switchResponse?.error ?? 0;
                    if (errorCode == 0)
                    {
                        return true;
                    }
                    else
                    {
                        var message = $"Switch request failed with error code: {errorCode}";
                        this.logger.LogInformation(message);
                        throw new LanControlRequestException(message, errorCode);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                this.logger.LogWarning("Switch request failed", e);
            }

            return false;
        }

        private string FormatInfoUrl(IPAddress ipAddress, int port) => FormatInfoUrl(ipAddress.ToString(), port);

        private string FormatInfoUrl(string ipAddress, int port) => FormatBaseUrl(ipAddress, port) + "info";

        private string FormatSwitchUrl(IPAddress ipAddress, int port, bool multiple) => FormatSwitchUrl(ipAddress.ToString(), port, multiple);

        private string FormatSwitchUrl(string ipAddress, int port, bool multiple) => FormatBaseUrl(ipAddress, port) + "switch" + (multiple ? "es" : string.Empty);

        private string FormatBaseUrl(string ipAddress, int port) => $"http://{ipAddress}:{port}/zeroconf/";

        private void OnAnswerReceived(object sender, MessageEventArgs e)
        {
            this.answerLock.Wait();
            try
            {
                var serviceRecord = e.Message.Answers.FirstOrDefault(a => a.Type == DnsType.SRV) as SRVRecord;
                var txtRecord = e.Message.Answers.FirstOrDefault(a => a.Type == DnsType.TXT) as TXTRecord;
                var aRecord = e.Message.Answers.FirstOrDefault(a => a.Type == DnsType.A) as ARecord;
                if (serviceRecord == null || txtRecord == null || aRecord == null)
                {
                    return;
                }

                var port = serviceRecord.Port;
                var ipAddress = aRecord.Address;
                var information = txtRecord.Strings.Where(x => !string.IsNullOrWhiteSpace(x) && x.Contains('=')).Select(st => st.Split('=')).ToDictionary(st => st[0], st => st[1]);
                if (information.Count == 0)
                {
                    return;
                }

                if (!information.TryGetValue("id", out var id) || !deviceCache.TryGetDevice(id, out var device))
                {
                    return;
                }

                bool encrypted = false;
                if (information.TryGetValue("encrypt", out var encryptValue))
                {
                    encrypted = encryptValue == "true";
                }

                if (!device!.HasLanControl)
                {
                    device.LanControl = new LanControlInformation(ipAddress, port, encrypted);
                }

                long seq = 0;
                if (information.TryGetValue("seq", out var seqValue))
                {
                    seq = long.Parse(seqValue);
                }

                var eventTime = new DateTimeOffset(DateTime.SpecifyKind(txtRecord.CreationTime, DateTimeKind.Local)); // this is in local time
                var hasPreviousSeq = this.deviceLastSeq.TryGetValue(id, out var previousSeq);
                if (previousSeq >= seq)
                {
                    return;
                }

                this.deviceLastSeq.AddOrUpdate(id, (_) => seq, (_, _) => seq);
                var json = GetData(information);
                if (encrypted)
                {
                    information.TryGetValue("iv", out var iv);
                    var deviceKey = device.DeviceKey;
                    if (deviceKey is null)
                    {
                        return;
                    }

                    json = Decrypt(deviceKey, iv, json);
                }

                // as this point we should have enough information to build an event parameter and raise it.
                var parametersJsonObject = JObject.Parse(json);
                var deviceUiid = device.Uiid;
                Type deviceType = this.deviceCache.GetEventParameterTypeForUiid(deviceUiid) ?? typeof(EventParameters);
                if (device is IDevice<Parameters> typedDevice)
                {
                    var updateResult = typedDevice.Parameters.Update(json);
                    if (typeof(IMultiSwitchEventParameters).IsAssignableFrom(deviceType) && updateResult.HasValue)
                    {
                        parametersJsonObject.Add("triggeredOutlet", updateResult.Value);
                    }
                }

                if (hasPreviousSeq && this.ParametersUpdated != null)
                {
                    if (typeof(EventParameters).IsAssignableFrom(deviceType))
                    {
                        if (!parametersJsonObject.ContainsKey("trigTime"))
                        {
                            parametersJsonObject.Add("trigTime", new JValue(eventTime.ToUnixTimeMilliseconds()));
                        }
                    }

                    var jsonObject = JObject.FromObject(new
                    {
                        action = "update",
                        deviceid = device.DeviceId,
                        uiid = device.Uiid,
                        eventSource = EventSource.Lan,
                    });

                    jsonObject.Add("params", parametersJsonObject);

                    var eventType = typeof(LinkEvent<>).MakeGenericType(deviceType);
                    if (jsonObject.ToObject(eventType) is ILinkEvent<IEventParameters> linkEvent)
                    {
                        this.ParametersUpdated(linkEvent);
                    }
                }
            }
            finally
            {
                this.answerLock.Release();
            }
        }

        private string GetData(Dictionary<string, string> information)
        {
            StringBuilder sb = new ();
            for (var i = 1; i <= 4; i++)
            {
                var dataKey = $"data{i}";
                if (!information.TryGetValue(dataKey, out var data))
                {
                    break;
                }

                sb.Append(data);
            }

            return sb.ToString();
        }

        private string Decrypt(string deviceKey, string iv, string encoded)
        {
            var eDeviceKey = Encoding.UTF8.GetBytes(deviceKey);
            using var md5 = MD5.Create();
            var key = md5.ComputeHash(eDeviceKey);

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = key.Length * 8;
            aes.KeySize = 128;
            aes.Padding = PaddingMode.None;

            byte[] Base64Decode(string input)
            {
                try
                {
                    return Convert.FromBase64String(input);
                }
                catch (FormatException)
                {
                }

                return Base64Decode(input + '=');
            }

            try
            {
                var ivBase64Decoded = Base64Decode(iv);
                using var decryptor = aes.CreateDecryptor(key, ivBase64Decoded);
                var ciphertext = Base64Decode(encoded);
                var data = PerformCryptography(decryptor, ciphertext);
                var paddingLength = data.Last();

                return Encoding.UTF8.GetString(data, 0, data.Length - paddingLength);
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        private PayLoad Encrypt(string deviceKey, string data)
        {
            var eDeviceKey = Encoding.UTF8.GetBytes(deviceKey);
            using var md5 = MD5.Create();
            var key = md5.ComputeHash(eDeviceKey);

            byte[] iv = new byte[16];
            Rand.NextBytes(iv);

            var ivBase64Encoded = Convert.ToBase64String(iv);

            var plaintext = Encoding.UTF8.GetBytes(data);
            using var aes = Aes.Create();
            using var encryptor = aes.CreateEncryptor(key, iv);
            var encryptedData = PerformCryptography(encryptor, plaintext);
            var dataEncoded = Convert.ToBase64String(encryptedData);
            return new PayLoad(true, ivBase64Encoded, dataEncoded);
        }

        private byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        private record PayLoad(bool encrypt, string? iv, string data);

        private record SwitchResponse(long seq, int error, dynamic data);
    }
}
