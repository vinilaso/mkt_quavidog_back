using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.AssignUserToTeam
{
    public record AssignUserToTeamCommand(Guid TeamId, Guid UserId, TeamMemberRole Role) : 
        ICommand<Result>, 
        ITeamManagerRequest;
}
