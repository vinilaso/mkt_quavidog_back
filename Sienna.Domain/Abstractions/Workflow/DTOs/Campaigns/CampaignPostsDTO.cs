namespace Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns
{
    public class CampaignPostsDTO
    {
        public Guid CampaignId { get; set; }
        public IEnumerable<CampaignPostDTO> Posts { get; set; } = [];
    }
}
