using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.DeclineInvitation
{
    public sealed record DeclineInvitationCommand(Guid InvitationId) : ICommand<Result>;
}
