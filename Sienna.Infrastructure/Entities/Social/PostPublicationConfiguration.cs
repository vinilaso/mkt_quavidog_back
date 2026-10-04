using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Identity;
using Sienna.Domain.Entities.Social;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Social
{
    internal sealed class PostPublicationConfiguration : BaseEntityConfiguration<PostPublication>
    {
        private const int EnumMaxLength = 32;

        protected override ModulePrefix Module => ModulePrefix.Social;

        protected override string TableName => "POST_PUBLICATIONS";

        protected override void ConfigureSpecific(EntityTypeBuilder<PostPublication> builder)
        {
            builder.HasKey(publication => publication.Id);

            builder.Property(publication => publication.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(publication => publication.Format)
                .HasConversion<string>()
                .HasMaxLength(EnumMaxLength)
                .IsRequired();

            builder.Property(publication => publication.Status)
                .HasConversion<string>()
                .HasMaxLength(EnumMaxLength)
                .IsRequired();

            builder.Property(publication => publication.ScheduledFor)
                .IsRequired();

            builder.Property(publication => publication.RequestedAt)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(publication => publication.RejectionReason)
                .HasMaxLength(PostPublication.MaxRejectionReasonLength);

            builder.Property(publication => publication.FailureReason)
                .HasMaxLength(PostPublication.MaxFailureReasonLength);

            builder.Property(publication => publication.ExternalIds)
                .IsRequired();

            #region Relacionamentos

            builder.HasOne(publication => publication.Post)
                .WithMany()
                .HasForeignKey(publication => publication.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(publication => publication.Team)
                .WithMany()
                .HasForeignKey(publication => publication.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(publication => publication.RequestedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(publication => publication.ReviewedById)
                .OnDelete(DeleteBehavior.SetNull);

            #endregion

            #region Índices

            builder.HasIndex(publication => new { publication.Status, publication.ScheduledFor });

            builder.HasIndex(publication => new { publication.TeamId, publication.ScheduledFor });

            builder.HasIndex(publication => new { publication.PostId, publication.Format })
                .IsUnique()
                .HasFilter("\"STATUS\" IN ('PendingApproval', 'Approved', 'Publishing', 'Published')");

            #endregion
        }
    }
}
