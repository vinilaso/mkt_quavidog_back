using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Workflow;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Workflow
{
    internal class TeamMemberConfiguration : BaseEntityConfiguration<TeamMember>
    {
        protected override ModulePrefix Module => ModulePrefix.Workflow;

        protected override string TableName => "TEAM_MEMBERS";

        protected override void ConfigureSpecific(EntityTypeBuilder<TeamMember> builder)
        {
            builder.HasKey(tm => new { tm.TeamId, tm.MemberId });

            builder.Property(tm => tm.TeamId)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(tm => tm.MemberId)
                .ValueGeneratedNever()
                .IsRequired();

            builder.HasOne(tm => tm.Team)
                .WithMany(team => team.Members)
                .HasForeignKey(tm => tm.TeamId);

            builder.HasOne(tm => tm.Member)
                .WithMany(member => member.Teams)
                .HasForeignKey(tm => tm.MemberId);
        }
    }
}
