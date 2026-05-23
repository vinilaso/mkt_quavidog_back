using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Identity;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Identity
{
    internal class UserConfiguration : BaseEntityConfiguration<User>
    {
        protected override ModulePrefix Module => ModulePrefix.Identity;

        protected override string TableName => "USERS";

        protected override void ConfigureSpecific(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(u => u.FullName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Navigation(u => u.Teams)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
