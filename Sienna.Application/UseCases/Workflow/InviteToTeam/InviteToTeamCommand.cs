using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.InviteToTeam
{
    public sealed record InviteToTeamCommand(Guid TeamId, string Email, TeamMemberRole Role)
        : ITeamManagerRequest, ICommand<Result>;
}
