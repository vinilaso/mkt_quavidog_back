using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Identity;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Identity
{
    internal class UserConfiguration : BaseEntityConfiguration<User>
    {
        protected override ModulePrefix Module => ModulePrefix.Identity;

        protected override void ConfigureSpecific(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FullName)
                .HasMaxLength(255)
                .IsRequired();
        }
    }
}
