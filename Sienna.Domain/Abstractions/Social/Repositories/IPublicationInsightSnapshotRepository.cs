using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.Repositories
{
    public interface IPublicationInsightSnapshotRepository : IAbstractRepository<PublicationInsightSnapshot>
    {
        Task<PublicationInsightsDTO?> GetPublicationInsightsAsync(Guid teamId, Guid publicationId, CancellationToken cancellationToken = default);
        Task<CampaignInsightsDataDTO?> GetCampaignInsightDataAsync(Guid teamId, Guid campaignId, CancellationToken cancellationToken = default);
    }
}
