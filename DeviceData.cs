namespace EWeLink.Api
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

                return deviceChannelCount ?? new Dictionary<string, int>();
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

                return deviceTypeUuid ?? new Dictionary<int, string>();
            }
        }

        private static void LoadData()
        {
            deviceChannelCount = LoadAndDeserializeResource<Dictionary<string, int>>("devices-channel-length.json");

            deviceTypeUuid = LoadAndDeserializeResource<Dictionary<int, string>>("devices-type-uuid.json");
        }

        private static T? LoadAndDeserializeResource<T>(string resourceName)
        {
            var assembly = typeof(DeviceData).GetTypeInfo().Assembly;
            var serializer = new JsonSerializer();
            var assemblyResourceName = assembly.GetManifestResourceNames().Single(x => x.EndsWith($".Resources.{resourceName}"));
            using var stream = assembly.GetManifestResourceStream(assemblyResourceName);
            using var reader = new StreamReader(stream);
            using var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<T>(jsonReader);
        }
    }
}