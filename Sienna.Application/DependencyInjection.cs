using Microsoft.Extensions.DependencyInjection;
using Sienna.Application.Behaviors.Teams;
using Sienna.Application.Messaging.Teams;
using Sienna.Application.UseCases.Social.Instagram;

namespace Sienna.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            AddMediatR(services);
            AddTeamScope(services);
            AddInstagramServices(services);

            return services;
        }

        private static void AddMediatR(IServiceCollection services)
        {
            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                c.AddOpenBehavior(typeof(TeamAuthorizationBehavior<,>));
            });
        }

        private static void AddTeamScope(IServiceCollection services)
        {
            services.AddScoped<TeamAccessContext>();
            services.AddScoped<ITeamAccessContext>(provider => provider.GetRequiredService<TeamAccessContext>());
        }

        private static void AddInstagramServices(IServiceCollection services)
        {
            services.AddScoped<InstagramCredentialsResolver>();
        }
    }
}
