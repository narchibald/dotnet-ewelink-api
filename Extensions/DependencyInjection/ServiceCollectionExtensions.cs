namespace EWeLink.Api.Extensions.DependencyInjection
{
    using System.Net;
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEWeLinkServices(this IServiceCollection services)
        {
            services.AddSingleton<IDeviceCache, DeviceCache>();
            services.AddScoped<ILink, Link>();
            services.AddScoped<ILinkWebSocket, LinkWebSocket>();
            services.AddScoped<ILinkLanControl, LinkLanControl>();
            services.AddHttpClient("eWeLink-lan")
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip,
                });
            services.AddHttpClient();

            return services;
        }
    }
}