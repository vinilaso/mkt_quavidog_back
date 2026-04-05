using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Email;
using System.Net.Http.Headers;

namespace Sienna.Infrastructure.Email.MailerSend
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMailerSendEmail(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<EmailSettings>()
                .Bind(configuration.GetSection(nameof(EmailSettings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddHttpClient<IEmailService, MailerSendService>((serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;

                client.BaseAddress = new Uri("https://api.mailersend.com/v1/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            return services;
        }
    }
}
