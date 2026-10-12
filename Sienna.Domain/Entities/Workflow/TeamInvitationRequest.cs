namespace Sienna.Domain.Entities.Workflow
{
    public class TeamInvitationRequest
    {
        public Guid TeamId { get; set; }
        public string Email { get; set; } = string.Empty;
        public TeamMemberRole Role { get; set; }
        public Guid InvitedById { get; set; }
        public DateTime UtcNow { get; set; }
    }
}
