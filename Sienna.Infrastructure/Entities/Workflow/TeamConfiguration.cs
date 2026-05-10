using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Workflow;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Workflow
{
    internal class TeamConfiguration : BaseEntityConfiguration<Team>
    {
        protected override ModulePrefix Module => ModulePrefix.Workflow;

        protected override string TableName => "TEAMS";

        protected override void ConfigureSpecific(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(team => team.Id);

            builder.Property(team => team.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(team => team.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(team => team.CreatedAt)
                .IsRequired();

            builder.Metadata.FindNavigation(nameof(Team.Members))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
