using System.Net.Http;

namespace EWeLink.Api
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using EWeLink.Api.Models;
    using EWeLink.Api.Models.Devices;
    using EWeLink.Api.Models.EventParameters;
    using EWeLink.Api.Models.Parameters;
    using Makaretu.Dns;
    using Newtonsoft.Json.Linq;

    public interface ILinkLanControl
    {
        event Action<ILinkEvent<IEventParameters>>? ParametersUpdated;

        void Start();

        void Stop();
    }

    public class LinkLanControl : ILinkLanControl, IDisposable
    {
        private static readonly HttpClient HttpClient = new ();
        private static readonly Random Rand = new ();
        private readonly IDeviceCache deviceCache;
        private readonly MulticastService multicastService = new ();
        private readonly ConcurrentDictionary<string, long> deviceLastSeq = new ();
        private readonly SemaphoreSlim answerLock = new (1, 1);
        private bool started;

        public LinkLanControl(IDeviceCache deviceCache)
        {
            this.deviceCache = deviceCache;
            multicastService.NetworkInterfaceDiscovered += (_, _) => multicastService.SendQuery("_ewelink._tcp");
            multicastService.AnswerReceived += OnAnswerReceived;
        }

        public event Action<ILinkEvent<IEventParameters>>? ParametersUpdated;

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

                var id = information["id"];
                if (!deviceCache.TryGetDevice(id, out var device))
                {
                    return;
                }

                var encrpted = information["encrypt"] == "true";
                if (!device!.HasLanControl)
                {
                    device.LanControl = new LanControlInformation(ipAddress, port, encrpted);
                }

                var seq = long.Parse(information["seq"]);
                var eventTime = new DateTimeOffset(DateTime.SpecifyKind(txtRecord.CreationTime, DateTimeKind.Local)); // this is in local time
                void UpdateLastSeq(string did, long s) => this.deviceLastSeq.AddOrUpdate(did, (i) => s, (i, p) => s);

                var hasPreviousSeq = this.deviceLastSeq.TryGetValue(id, out var previousSeq);
                if (previousSeq >= seq)
                {
                    return;
                }

                this.deviceLastSeq.AddOrUpdate(id, (_) => seq, (_, _) => seq);
                var json = GetData(information);
                if (encrpted)
                {
                    var iv = information["iv"];
                    var deviceKey = device!.DeviceKey;
                    json = Decrypt(deviceKey, iv, json);
                }

                // as this point we should have enough information to build an event parameter and raise it.
                var parametersJsonObject = JObject.Parse(json);
                var deviceUiid = device!.Uiid;
                Type deviceType = this.deviceCache.GetEventParameterTypeForUiid(deviceUiid) ?? typeof(EventParameters);
                if (device is IDevice<Parameters> typedDevice)
                {
                    typedDevice.Parameters.Update(json.ToString());
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
                        deviceid = device.Id,
                        uiid = device.Uiid,
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

        private string Decrypt(string devicekey, string iv, string encoded)
        {
            var eDevicekey = Encoding.UTF8.GetBytes(devicekey);
            using var md5 = MD5.Create();
            var key = md5.ComputeHash(eDevicekey);

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = key.Length * 8;
            aes.KeySize = 128;
            aes.Padding = PaddingMode.None;


            byte[] Base64Decode(string input)
            {
                try{
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

        private PayLoad Encrypt(string devicekey, string data)
        {
            byte[] Pad(byte[] data_to_pad, int block_size)
            {
                var padding_len = block_size - data_to_pad.Length % block_size;

                var padded = new byte[padding_len + data_to_pad.Length];
                Array.Copy(data_to_pad, padded, data_to_pad.Length);
                padded[padded.Length - 1] = Convert.ToByte(padding_len);
                return padded;
            }


            var eDevicekey = Encoding.UTF8.GetBytes(devicekey);
            using var md5 = MD5.Create();
            var key = md5.ComputeHash(eDevicekey);

            byte[] iv = new byte[16];
            Rand.NextBytes(iv);

            var ivBase64Encoded = Convert.ToBase64String(iv);

            var plaintext = Encoding.UTF8.GetBytes(data);
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.BlockSize = key.Length * 8;
            aes.KeySize = 128;
            aes.Padding = PaddingMode.None;

            try
            {
                var paddedPlaintext = Pad(plaintext, aes.BlockSize / 8); 
                using var encryptor = aes.CreateEncryptor(key, iv);
                var encrptedData = PerformCryptography(encryptor, paddedPlaintext);
                var dataEncoded = Convert.ToBase64String(encrptedData);
                return new PayLoad(true, ivBase64Encoded, dataEncoded);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private byte[] PerformCryptography(ICryptoTransform cryptoTransform, byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }

        private record PayLoad(bool encrypt, string iv, string data);
    }
}
