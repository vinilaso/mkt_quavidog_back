namespace Sienna.Domain.Abstractions.Workflow.DTOs.Teams
{
    public class TeamMemberDTO
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
}
