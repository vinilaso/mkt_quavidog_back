using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Media
{
    internal class MediaConfiguration : BaseEntityConfiguration<Domain.Entities.Media.Media>
    {
        protected override ModulePrefix Module => ModulePrefix.Media;
        protected override string TableName => "MEDIAS";

        protected override void ConfigureSpecific(EntityTypeBuilder<Domain.Entities.Media.Media> builder)
        {
            builder.HasKey(media => media.Id);

            builder.Property(media => media.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(media => media.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(media => media.Extension)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(media => media.Content)
                .IsRequired();
        }
    }
}
