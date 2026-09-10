namespace Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns
{
    public class CampaignPostDTO
    {
        public Guid PostId { get; set; }
        public string? Caption { get; set; }
        public string? Status { get; set; }
    }
}
