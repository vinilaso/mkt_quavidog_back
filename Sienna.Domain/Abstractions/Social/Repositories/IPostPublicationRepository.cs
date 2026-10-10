using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.Repositories
{
    public interface IPostPublicationRepository : IAbstractRepository<PostPublication>
    {
        Task<bool> HasBlockingPublicationAsync(Guid postId, PublicationFormat format, CancellationToken cancellationToken = default);
        Task<PostPublication?> FindForPublishingAsync(Guid publicationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Guid>> ClaimDueAsync(DateTime utcNow, int maxCount, CancellationToken cancellationToken = default);
        Task<int> FailInterruptedAsync(string reason, CancellationToken cancellationToken = default);
        Task<PostPublication?> FindTeamPublicationByIdAsync(Guid teamId, Guid publicationId, CancellationToken cancellationToken = default);
    }
}
