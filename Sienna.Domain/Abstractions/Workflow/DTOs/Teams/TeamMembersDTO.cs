namespace Sienna.Domain.Abstractions.Workflow.DTOs.Teams
{
    public class TeamMembersDTO
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public IEnumerable<TeamMemberDTO> Members { get; set; } = [];
    }
}
