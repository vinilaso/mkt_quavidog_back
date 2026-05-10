using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sienna.Application.Interfaces;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Identity;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow;
using Sienna.Domain.Entities.Identity;
using Sienna.Infrastructure.Authentication;
using Sienna.Infrastructure.Database;
using Sienna.Infrastructure.Email.Queue;
using Sienna.Infrastructure.Email.Resend;
using Sienna.Infrastructure.Repositories.Identity;
using Sienna.Infrastructure.Repositories.Workflow;
using Sienna.Infrastructure.Security;

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

            services.AddResendService(configuration);

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

            services.AddSingleton<IEmailQueue, InMemoryEmailQueue>(services => new InMemoryEmailQueue(500));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
        }
    }
}
