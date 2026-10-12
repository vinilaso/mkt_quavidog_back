using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.AcceptInvitation
{
    public sealed record AcceptInvitationCommand(Guid InvitationId)
        : ICommand<Result<Guid>>;
}
