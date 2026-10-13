using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Infrastructure.Repositories.Social
{
    internal sealed class PublicationInsightSnapshotRepository(ApplicationContext context) : AbstractRepository<PublicationInsightSnapshot>(context), IPublicationInsightSnapshotRepository
    {
        public async Task<PublicationInsightsDTO?> GetPublicationInsightsAsync(Guid teamId, Guid publicationId, CancellationToken cancellationToken = default)
        {
            var publication = await Context.Set<PostPublication>()
                .Where(p => p.Id == publicationId && p.TeamId == teamId)
                .FirstOrDefaultAsync(cancellationToken);

            if (publication is null)
                return null;

            var timeline = await Context.Set<PublicationInsightSnapshot>()
                .Where(snapshot => snapshot.PublicationId == publicationId)
                .OrderBy(snapshot => snapshot.CollectedAt)
                .Select(snapshot => new InsightPointDTO
                {
                    CollectedAt = snapshot.CollectedAt,
                    Views = snapshot.Views,
                    Reach = snapshot.Reach,
                    Likes = snapshot.Likes,
                    Comments = snapshot.Comments,
                    Shares = snapshot.Shares,
                    Saved = snapshot.Saved,
                    Replies = snapshot.Replies,
                    TotalInteractions = snapshot.TotalInteractions
                })
                .ToListAsync(cancellationToken);

            var latest = timeline.LastOrDefault();

            return new PublicationInsightsDTO
            {
                PublicationId = publication.Id,
                Format = publication.Format,
                Status = publication.Status,
                PublishedAt = publication.PublishedAt,
                LastCollectedAt = latest?.CollectedAt,
                Totals = latest is null ? null : InsightTotalsDTO.From(latest),
                Timeline = timeline
            };
        }
    }
}
