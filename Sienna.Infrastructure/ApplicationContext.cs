using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Entities.Identity;
using Sienna.Infrastructure.Extensions;

namespace Sienna.Infrastructure
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            builder.ApplySnakeCaseUpperConvention();

            ApplyEFIdentityConfiguration(builder);
        }

        private static void ApplyEFIdentityConfiguration(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<Guid>>().ToTable("IDENTITY_ROLE");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("IDENTITY_USER_ROLE");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("IDENTITY_USER_CLAIM");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("IDENTITY_USER_LOGIN");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("IDENTITY_ROLE_CLAIM");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("IDENTITY_USER_TOKEN");
        }
    }
}
