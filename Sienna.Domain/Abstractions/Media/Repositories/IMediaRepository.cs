using MediaEntity = Sienna.Domain.Entities.Media.Media;

namespace Sienna.Domain.Abstractions.Media.Repositories
{
    public interface IMediaRepository : IAbstractRepository<MediaEntity>
    {
        Task<Dictionary<Guid, string>> GetExtensionsAsync(IEnumerable<Guid> mediaIds, CancellationToken cancellationToken = default);
    }
}
