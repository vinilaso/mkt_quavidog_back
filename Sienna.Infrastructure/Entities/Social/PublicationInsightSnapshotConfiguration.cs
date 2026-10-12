using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Social;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Social
{
    internal sealed class PublicationInsightSnapshotConfiguration : BaseEntityConfiguration<PublicationInsightSnapshot>
    {
        protected override ModulePrefix Module => ModulePrefix.Social;

        protected override string TableName => "PUBLICATION_INSIGHT_SNAPSHOTS";

        protected override void ConfigureSpecific(EntityTypeBuilder<PublicationInsightSnapshot> builder)
        {
            builder.HasKey(snapshot => snapshot.Id);
            builder.Property(snapshot => snapshot.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(snapshot => snapshot.CollectedAt)
                .IsRequired();

            builder.Property(snapshot => snapshot.Views)
                .ValueGeneratedNever();

            builder.Property(snapshot => snapshot.Reach)
                .ValueGeneratedNever();

            builder.Property(snapshot => snapshot.Likes)
                .ValueGeneratedNever();

            builder.Property(snapshot => snapshot.Comments)
                .ValueGeneratedNever();

            builder.Property(snapshot => snapshot.Shares)
                .ValueGeneratedNever();

            builder.Property(snapshot => snapshot.Saved)
                .ValueGeneratedNever();

            builder.Property(snapshot => snapshot.Replies)
                .ValueGeneratedNever();

            builder.Property(snapshot => snapshot.TotalInteractions)
                .ValueGeneratedNever();

            builder.Ignore(snapshot => snapshot.EngagementRate);

            builder.HasOne(snapshot => snapshot.Publication)
                .WithMany()
                .HasForeignKey(snapshot => snapshot.PublicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(snapshot => new { snapshot.PublicationId, snapshot.CollectedAt });
        }
    }
}
