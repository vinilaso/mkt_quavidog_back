using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Infrastructure.Repositories.Social.Instagram
{
    internal class InstagramAccountRepository(ApplicationContext applicationContext) : AbstractRepository<InstagramAccount>(applicationContext), IInstagramAccountRepository
    {
        public async Task<InstagramAccount?> FindByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<InstagramAccount>()
                .Where(account => account.TeamId == teamId)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
