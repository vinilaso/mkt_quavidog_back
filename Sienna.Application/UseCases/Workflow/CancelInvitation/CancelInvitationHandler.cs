using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.CancelInvitation
{
    public sealed class CancelInvitationHandler(ITeamInvitationRepository invitationRepository, IUnitOfWork uow) : IRequestHandler<CancelInvitationCommand, Result>
    {
        public async Task<Result> Handle(CancelInvitationCommand request, CancellationToken cancellationToken)
        {
            if (await invitationRepository.FindTeamInvitationAsync(request.TeamId, request.InvitationId, cancellationToken) is not TeamInvitation invitation)
                return InvitationErrors.NotFound;

            var canceled = invitation.Cancel(DateTime.UtcNow);

            if (canceled.IsFailure)
                return canceled.Error;

            await uow.CommitChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
