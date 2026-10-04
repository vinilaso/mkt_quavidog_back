using Microsoft.Extensions.DependencyInjection;
using Sienna.Application.Behaviors.Teams;

namespace Sienna.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                c.AddOpenBehavior(typeof(TeamAuthorizationBehavior<,>));
            });

            return services;
        }
    }
}
