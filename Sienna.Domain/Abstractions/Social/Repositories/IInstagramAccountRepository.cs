using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.Repositories
{
    public interface IInstagramAccountRepository : IAbstractRepository<InstagramAccount>
    {
        Task<InstagramAccount?> FindByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default);
    }
}
