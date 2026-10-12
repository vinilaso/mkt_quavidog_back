using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.DTOs
{
    public class PublicationNotificationDTO
    {
        public Guid PublicationId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public PublicationFormat Format { get; set; }
        public PublicationStatus Status { get; set; }
        public DateTime ScheduledFor { get; set; }
        public string RequesterName { get; set; } = string.Empty;
        public string RequesterEmail { get; set; } = string.Empty;
        public string? ReviewerName { get; set; }
        public string? RejectionReason { get; set; }
        public string? FailureReason { get; set; }
    }
}
