using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Media.Repositories;
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
    }
}
