using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Workflow;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Workflow
{
    internal sealed class CampaignPostsConfiguration : BaseEntityConfiguration<CampaignPost>
    {
        protected override ModulePrefix Module => ModulePrefix.Workflow;

        protected override string TableName => "CAMPAIGN_POSTS";

        protected override void ConfigureSpecific(EntityTypeBuilder<CampaignPost> builder)
        {
            builder.HasKey(post => new { post.PostId, post.CampaignId });

            builder.Property(post => post.AssignmentDate);

            builder.Property(post => post.PostId)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(post => post.CampaignId)
                .ValueGeneratedNever()
                .IsRequired();

            builder.HasOne(post => post.Post)
                .WithMany(post => post.Campaigns)
                .HasForeignKey(post => post.PostId);

            builder.HasOne(post => post.Campaign)
                .WithMany(campaign => campaign.Posts)
                .HasForeignKey(post => post.CampaignId);
        }
    }
}
