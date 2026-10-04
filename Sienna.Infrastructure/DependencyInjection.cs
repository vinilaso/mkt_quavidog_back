using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sienna.Application.Interfaces;
using Sienna.Application.Interfaces.Email;
using Sienna.Application.Interfaces.Security;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Abstractions.Identity.Services;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Identity;
using Sienna.Infrastructure.Authentication;
using Sienna.Infrastructure.Database;
using Sienna.Infrastructure.Email.Queue;
using Sienna.Infrastructure.Email.Resend;
using Sienna.Infrastructure.Repositories.Identity;
using Sienna.Infrastructure.Repositories.Media;
using Sienna.Infrastructure.Repositories.Social;
using Sienna.Infrastructure.Repositories.Social.Instagram;
using Sienna.Infrastructure.Repositories.Workflow;
using Sienna.Infrastructure.Security;
using Sienna.Infrastructure.Security.SecretProtection;
using Sienna.Infrastructure.Social.Instagram;

namespace Sienna.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDataBase(services, configuration);
            AddIdentity(services);
            AddLocalServices(services);
            AddRepositories(services);
            AddSecurity(services, configuration);

            services.AddResendService(configuration);
            services.AddInstagramServices(configuration);

            return services;
        }

        private static void AddDataBase(IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                ?? configuration.GetConnectionString("DATABASE_URL")
                ?? throw new InvalidOperationException("Connection string is not set.");

            services.AddDbContext<ApplicationContext>(
                options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Sienna.Infrastructure"))
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = string.Empty;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
        }

        private static void AddLocalServices(IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUserContext, HttpContextUserContext>();
            services.AddScoped<ISecretProvider, AesSecretProvider>();

            services.AddSingleton<IEmailQueue, InMemoryEmailQueue>(services => new InMemoryEmailQueue(500));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICampaignRepository, CampaignRepository>();
            services.AddScoped<IInstagramAccountRepository, InstagramAccountRepository>();
            services.AddScoped<IPostPublicationRepository, PostPublicationRepository>();
        }

        private static void AddSecurity(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<SecretProviderSettings>()
                .Bind(configuration.GetSection(nameof(SecretProviderSettings)))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
