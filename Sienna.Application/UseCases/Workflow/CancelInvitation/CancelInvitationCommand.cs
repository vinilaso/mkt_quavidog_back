using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.CancelInvitation
{
    public sealed record CancelInvitationCommand(Guid TeamId, Guid InvitationId)
        : ITeamManagerRequest, ICommand<Result>;
}
