using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Media;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Media
{
    internal sealed class PostAssetConfiguration : BaseEntityConfiguration<PostAsset>
    {
        protected override ModulePrefix Module => ModulePrefix.Media;

        protected override string TableName => "POST_ASSETS";

        protected override void ConfigureSpecific(EntityTypeBuilder<PostAsset> builder)
        {
            builder.HasKey(asset => new { asset.PostId, asset.MediaId });

            builder.Property(asset => asset.SequenceOrder);

            builder.Property(asset => asset.PostId)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(asset => asset.MediaId)
                .ValueGeneratedNever()
                .IsRequired();

            builder.HasOne(asset => asset.Post)
                .WithMany(post => post.Assets)
                .HasForeignKey(asset => asset.PostId);

            builder.HasOne(asset => asset.Media)
                .WithMany(media => media.Assets)
                .HasForeignKey(asset => asset.MediaId);
        }
    }
}
