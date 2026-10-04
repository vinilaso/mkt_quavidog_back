using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Infrastructure.Repositories.Social
{
    internal sealed class PostPublicationRepository(ApplicationContext applicationContext) : AbstractRepository<PostPublication>(applicationContext), IPostPublicationRepository
    {
        private static readonly PublicationStatus[] BlockingStatuses =
        [
            PublicationStatus.PendingApproval,
            PublicationStatus.Approved,
            PublicationStatus.Publishing,
            PublicationStatus.Published
        ];

        public async Task<IReadOnlyList<Guid>> ClaimDueAsync(DateTime utcNow, int maxCount, CancellationToken cancellationToken = default)
        {
            var candidateIds = await Context.Set<PostPublication>()
                .Where(publication => publication.Status == PublicationStatus.Approved && publication.ScheduledFor <= utcNow)
                .OrderBy(publication => publication.ScheduledFor)
                .Select(publication => publication.Id)
                .Take(maxCount)
                .ToListAsync(cancellationToken);

            List<Guid> claimed = [];

            foreach (var id in candidateIds)
            {
                var affected = await Context.Set<PostPublication>()
                    .Where(publication => publication.Id == id && publication.Status == PublicationStatus.Approved)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(publication => publication.Status, PublicationStatus.Publishing), cancellationToken);

                if (affected == 1)
                    claimed.Add(id);
            }

            return claimed;
        }

        public async Task<int> FailInterruptedAsync(string reason, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostPublication>()
                .Where(publication => publication.Status == PublicationStatus.Publishing)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(publication => publication.Status, PublicationStatus.Failed)
                    .SetProperty(publication => publication.FailureReason, reason), 
                    cancellationToken);
        }

        public async Task<PostPublication?> FindForPublishingAsync(Guid publicationId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostPublication>()
                .Include(publication => publication.Post)
                    .ThenInclude(post => post!.Assets)
                .FirstOrDefaultAsync(publication => publication.Id == publicationId, cancellationToken);
        }

        public async Task<bool> HasBlockingPublicationAsync(Guid postId, PublicationFormat format, CancellationToken cancellationToken = default)
        {
            return await Context.Set<PostPublication>()
                .AnyAsync(publication =>
                    publication.PostId == postId &&
                    publication.Format == format &&
                    BlockingStatuses.Contains(publication.Status),
                    cancellationToken);
        }
    }
}
