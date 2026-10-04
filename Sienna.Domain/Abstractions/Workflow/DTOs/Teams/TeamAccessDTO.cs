using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.DTOs.Teams
{
    public class TeamAccessDTO
    {
        public Guid TeamId { get; init; }
        public TeamMemberRole? Role { get; init; }
    }
}
