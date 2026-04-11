using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sienna.Application.Interfaces.Email;

namespace Sienna.Infrastructure.Email.MailKit
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMailKitService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<MailKitSettings>()
                .Bind(configuration.GetSection(nameof(MailKitSettings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddTransient<IEmailService, MailKitService>();

            return services;
        }
    }
}
