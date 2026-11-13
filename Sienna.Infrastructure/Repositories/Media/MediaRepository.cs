using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Entities.Media;
using MediaEntity = Sienna.Domain.Entities.Media.Media;

namespace Sienna.Infrastructure.Repositories.Media
{
    internal class MediaRepository(ApplicationContext context) : AbstractRepository<MediaEntity>(context), IMediaRepository
    {
        public async Task<Dictionary<Guid, string>> GetExtensionsAsync(IEnumerable<Guid> mediaIds, CancellationToken cancellationToken = default)
        {
            var ids = mediaIds.ToList();

            return await Context.Set<MediaEntity>()
                .Where(media => ids.Contains(media.Id))
                .ToDictionaryAsync(media => media.Id, media => media.Extension, cancellationToken);
        }

        public async Task<bool> IsLinkedToOtherPostsAsync(Guid mediaId, Guid postId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostAsset>()
                .AnyAsync(asset => asset.MediaId == mediaId && asset.PostId != postId, cancellationToken);
        }
    }
}
