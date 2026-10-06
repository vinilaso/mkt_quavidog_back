namespace Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns
{
    /// <summary>Postagens de uma campanha.</summary>
    public class CampaignPostsDTO
    {
        /// <summary>ID da campanha.</summary>
        public Guid CampaignId { get; set; }
        /// <summary>Postagens associadas à campanha.</summary>
        public IEnumerable<CampaignPostDTO> Posts { get; set; } = [];
    }
}
