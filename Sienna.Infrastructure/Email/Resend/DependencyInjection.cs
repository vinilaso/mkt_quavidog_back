using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Email;
using System.Net.Http.Headers;

namespace Sienna.Infrastructure.Email.Resend
{
    internal static class DependencyInjection
    {
        internal static IServiceCollection AddResendService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ResendSettings>()
                .Bind(configuration.GetSection(nameof(ResendSettings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<IEmailService, ResendService>((serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<ResendSettings>>().Value;

                client.BaseAddress = new Uri("https://api.resend.com");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("sienna-webapi", "1.0"));
            });

            return services;
        }
    }
}
