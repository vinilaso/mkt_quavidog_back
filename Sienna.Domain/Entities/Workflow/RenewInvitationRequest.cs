namespace Sienna.Domain.Entities.Workflow
{
    public class RenewInvitationRequest
    {
        public TeamMemberRole Role { get; set; }
        public Guid InvitedById { get; set; }
        public DateTime UtcNow { get; set; }
    }
}
