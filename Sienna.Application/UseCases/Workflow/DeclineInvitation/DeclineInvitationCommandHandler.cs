using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.DeclineInvitation
{
    public sealed class DeclineInvitationCommandHandler(
        IUserContext userContext,
        ITeamInvitationRepository invitationRepository,
        IUnitOfWork uow) : IRequestHandler<DeclineInvitationCommand, Result>
    {
        public async Task<Result> Handle(DeclineInvitationCommand request, CancellationToken cancellationToken)
        {
            if (await invitationRepository.FindByIdAsync(request.InvitationId, cancellationToken) is not TeamInvitation invitation)
                return InvitationErrors.NotFound;

            var declined = invitation.Decline(userContext.Email, DateTime.UtcNow);

            if (declined.IsFailure)
                return declined.Error;

            await uow.CommitChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
