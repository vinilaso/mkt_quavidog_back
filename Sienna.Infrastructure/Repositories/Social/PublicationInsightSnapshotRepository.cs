using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;
using Sienna.Domain.Entities.Workflow;

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

        public async Task<CampaignInsightsDataDTO?> GetCampaignInsightDataAsync(Guid teamId, Guid campaignId, CancellationToken cancellationToken = default)
        {
            var campaign = await Context.Set<Campaign>()
                .Where(c => c.TeamId == teamId && c.Id == campaignId)
                .Select(c => new { c.Id, c.Name })
                .FirstOrDefaultAsync(cancellationToken);

            if (campaign is null)
                return null;

            var publications = await Context.Set<PostPublication>()
                .Where(publication => publication.TeamId == teamId
                    && publication.Status == PublicationStatus.Published
                    && publication.PublishedAt != null
                    && publication.Post!.Campaigns.Any(campaignPost => campaignPost.CampaignId == campaignId)
                )
                .OrderBy(publication => publication.PublishedAt)
                .ThenBy(publication => publication.Id)
                .Select(publication => new CampaignPublicationInsightDataDTO
                {
                    PublicationId = publication.Id,
                    PostId = publication.PostId,
                    Caption = publication.Post!.Caption,
                    Format = publication.Format,
                    PublishedAt = publication.PublishedAt!.Value,

                    Snapshots = Context.Set<PublicationInsightSnapshot>()
                        .Where(snapshot => snapshot.PublicationId == publication.Id)
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
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return new CampaignInsightsDataDTO
            {
                CampaignId = campaign.Id,
                CampaignName = campaign.Name,
                Publications = publications
            };
        }
    }
}
