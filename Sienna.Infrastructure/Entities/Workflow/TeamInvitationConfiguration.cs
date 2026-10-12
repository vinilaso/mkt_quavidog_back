using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Identity;
using Sienna.Domain.Entities.Workflow;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Workflow
{
    internal sealed class TeamInvitationConfiguration : BaseEntityConfiguration<TeamInvitation>
    {
        protected override ModulePrefix Module => ModulePrefix.Workflow;

        protected override string TableName => "TEAM_INVITATIONS";

        protected override void ConfigureSpecific(EntityTypeBuilder<TeamInvitation> builder)
        {
            builder.HasKey(invitation => invitation.Id);
            builder.Property(invitation => invitation.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(invitation => invitation.Email)
                .HasMaxLength(TeamInvitation.MaxEmailLength)
                .IsRequired();

            builder.Property(invitation => invitation.Role)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(invitation => invitation.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(invitation => invitation.CreatedAt)
                .IsRequired();

            builder.Property(invitation => invitation.ExpiresAt)
                .IsRequired();

            builder.HasOne(invitation => invitation.Team)
                .WithMany()
                .HasForeignKey(invitation => invitation.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(invitation => invitation.InvitedById)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(invitation => new { invitation.TeamId, invitation.Email })
                .IsUnique()
                .HasFilter("\"STATUS\" = 'Pending'");

            builder.HasIndex(invitation => new { invitation.Email, invitation.Status });
        }
    }
}
