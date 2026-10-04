using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.Messaging.Teams
{
    public interface ITeamAccessContext
    {
        Guid TeamId { get; }
        TeamMemberRole Role { get; }
        bool IsManager { get; }
    }
}
