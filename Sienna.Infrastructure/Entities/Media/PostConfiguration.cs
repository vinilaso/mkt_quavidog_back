using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sienna.Domain.Entities.Media;
using Sienna.Infrastructure.Database;

namespace Sienna.Infrastructure.Entities.Media
{
    internal sealed class PostConfiguration : BaseEntityConfiguration<Post>
    {
        protected override ModulePrefix Module => ModulePrefix.Media;

        protected override string TableName => "POSTS";

        protected override void ConfigureSpecific(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(post => post.Id);

            builder.Property(post => post.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(post => post.Caption);

            builder.Property(post => post.Status);

            builder.Property(post => post.CreatedAt)
                .ValueGeneratedNever()
                .IsRequired();

            builder.HasOne(post => post.Author)
                .WithMany(user => user.Posts)
                .HasForeignKey(post => post.AuthorId);
        }
    }
}
