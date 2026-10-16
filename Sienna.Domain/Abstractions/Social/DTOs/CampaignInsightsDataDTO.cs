namespace Sienna.Domain.Abstractions.Social.DTOs
{
    public class CampaignInsightsDataDTO
    {
        public Guid CampaignId { get; set; }
        public string CampaignName { get; set; } = string.Empty;
        public IReadOnlyList<CampaignPublicationInsightDataDTO> Publications { get; set; } = [];
    }
}
