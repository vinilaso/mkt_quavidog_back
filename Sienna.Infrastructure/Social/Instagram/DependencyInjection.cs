using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Social.Instagram;

namespace Sienna.Infrastructure.Social.Instagram
{
    internal static class DependencyInjection
    {
        internal static void AddInstagramServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<InstagramSettings>()
                .Bind(configuration.GetSection(nameof(InstagramSettings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<IInstagramClient, InstagramClient>((serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<InstagramSettings>>().Value;

                client.BaseAddress = new Uri(settings.GraphBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(90);
            });
        }
    }
}
