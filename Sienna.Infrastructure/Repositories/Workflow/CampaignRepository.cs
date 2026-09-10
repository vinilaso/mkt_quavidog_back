using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Infrastructure.Repositories.Workflow
{
    internal class CampaignRepository(ApplicationContext context) : AbstractRepository<Campaign>(context), ICampaignRepository
    {
        public async Task<CampaignPostsDTO?> GetCampaignPostsAsync(Guid campaignId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Campaign>()
                .Where(campaign => campaign.Id == campaignId)
                .Select(campaign => new CampaignPostsDTO
                {
                    CampaignId = campaign.Id,
                    Posts = campaign.Posts.Select(post => new CampaignPostDTO
                    {
                        PostId = post.PostId!.Value,
                        Caption = post.Post!.Caption,
                        Status = post.Post.Status.ToString()
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
