using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDataBase(services, configuration);
            AddIdentity(services);

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
        }

        private static void AddIdentity(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = string.Empty;
            })
            .AddEntityFrameworkStores<ApplicationContext>()
            .AddDefaultTokenProviders();
        }
    }
}
