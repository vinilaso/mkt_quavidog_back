using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Workflow;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Workflow
{
    internal class CampaignConfiguration : BaseEntityConfiguration<Campaign>
    {
        protected override ModulePrefix Module => ModulePrefix.Workflow;

        protected override string TableName => "CAMPAIGNS";

        protected override void ConfigureSpecific(EntityTypeBuilder<Campaign> builder)
        {
            builder.HasKey(campaign => campaign.Id);

            builder.Property(campaign => campaign.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(campaign => campaign.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(campaign => campaign.CreatedAt)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(campaign => campaign.Status);

            builder.HasOne(campaign => campaign.Team)
                .WithMany(team => team.Campaigns)
                .HasForeignKey(campaign => campaign.TeamId);
        }
    }
}
