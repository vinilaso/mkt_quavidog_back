using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.DTOs
{
    public class CampaignPublicationInsightDataDTO
    {
        public Guid PublicationId { get; set; }
        public Guid PostId { get; set; }
        public string Caption { get; set; } = string.Empty;
        public PublicationFormat Format { get; set; }
        public DateTime PublishedAt { get; set; }
        public IReadOnlyList<InsightPointDTO> Snapshots { get; set; } = [];
    }
}
