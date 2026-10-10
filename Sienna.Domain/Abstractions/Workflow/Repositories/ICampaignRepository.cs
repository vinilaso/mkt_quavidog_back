using Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.Repositories
{
    public interface ICampaignRepository : IAbstractRepository<Campaign>
    {
        Task<CampaignPostsDTO?> GetCampaignPostsAsync(Guid teamId, Guid campaignId, CancellationToken cancellationToken = default);
    }
}
