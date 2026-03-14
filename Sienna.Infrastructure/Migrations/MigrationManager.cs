using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Sienna.Infrastructure.Migrations
{
    public static class MigrationManager
    {
        public static IServiceProvider ForceMigration(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            scope.ServiceProvider.GetRequiredService<ApplicationContext>().Database.Migrate();

            return provider;
        }
    }
}
