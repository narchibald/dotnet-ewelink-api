namespace EWeLink.Api
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Newtonsoft.Json;

    internal static class DeviceData
    {
        private static Dictionary<string, int>? deviceChannelCount;

        private static Dictionary<int, string>? deviceTypeUuid;

        public static Dictionary<string, int> DeviceChannelCount
        {
            get
            {
                if (deviceChannelCount == null)
                {
                    LoadData();
                }

                return deviceChannelCount;
            }
        }

        public static Dictionary<int, string> DeviceTypeUuid
        {
            get
            {
                if (deviceTypeUuid == null)
                {
                    LoadData();
                }

                return deviceTypeUuid;
            }
        }

        private static void LoadData()
        {
            deviceChannelCount = LoadAndDeserializeResource<Dictionary<string, int>>("devices-channel-length.json");

            deviceTypeUuid = LoadAndDeserializeResource<Dictionary<int, string>>("devices-type-uuid.json");
        }

        private static T LoadAndDeserializeResource<T>(string resourceName)
        {
            var assembly = typeof(DeviceData).GetTypeInfo().Assembly;
            var serializer = new JsonSerializer();
            using (var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Resources.{resourceName}"))
            {
                using (var reader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(reader))
                    {
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
        }
    }
}