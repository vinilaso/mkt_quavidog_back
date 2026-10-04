using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Social;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Social
{
    internal class InstagramAccountConfiguration : BaseEntityConfiguration<InstagramAccount>
    {
        protected override ModulePrefix Module => ModulePrefix.Social;

        protected override string TableName => "INSTAGRAM_ACCOUNTS";

        protected override void ConfigureSpecific(EntityTypeBuilder<InstagramAccount> builder)
        {
            builder.HasKey(account => account.Id);

            builder.Property(account => account.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(account => account.TeamId)
                .IsRequired();

            builder.HasIndex(account => account.TeamId)
                .IsUnique();

            builder.Property(account => account.InstagramUserId)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(account => account.Username)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(account => account.ProtectedAccessToken)
                .IsRequired();

            builder.Property(account => account.ConnectedAt)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(account => account.UpdatedAt)
                .IsRequired();

            builder.HasOne(account => account.Team)
               .WithOne()
               .HasForeignKey<InstagramAccount>(account => account.TeamId);
        }
    }
}
