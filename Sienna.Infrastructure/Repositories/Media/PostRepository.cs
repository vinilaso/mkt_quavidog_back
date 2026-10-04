using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Entities.Media;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Infrastructure.Repositories.Media
{
    internal sealed class PostRepository(ApplicationContext context) : AbstractRepository<Post>(context), IPostRepository
    {
        public async Task<bool> BelongsToTeamAsync(Guid postId, Guid teamId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<CampaignPost>()
                .AnyAsync(campaignPost =>
                    campaignPost.PostId == postId &&
                    campaignPost.Campaign!.TeamId == teamId,
                    cancellationToken);
        }

        public override async Task<Post?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Post>()
                .Include(post => post.Assets)
                .FirstOrDefaultAsync(post => post.Id == id, cancellationToken);
        }
    }
}
