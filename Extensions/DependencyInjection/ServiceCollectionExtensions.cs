namespace EWeLink.Api.Extensions.DependencyInjection
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEWeLinkServices(this IServiceCollection services)
        {
            services.AddSingleton<IDeviceCache, DeviceCache>();
            services.AddScoped<ILink, Link>();
            services.AddScoped<ILinkWebSocket, LinkWebSocket>();
            services.AddScoped<ILinkLanControl, LinkLanControl>();
            services.AddHttpClient();

            return services;
        }
    }
}