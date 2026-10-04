using Sienna.Domain.Entities.Media;

namespace Sienna.Domain.Abstractions.Media.Repositories
{
    public interface IPostRepository : IAbstractRepository<Post>
    {
        Task<bool> BelongsToTeamAsync(Guid postId, Guid teamId, CancellationToken cancellationToken = default);
    }
}
