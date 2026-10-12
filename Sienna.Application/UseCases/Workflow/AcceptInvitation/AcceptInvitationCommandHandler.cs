using MediatR;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;
using Sienna.Domain.Exceptions.Persistence;

namespace Sienna.Application.UseCases.Workflow.AcceptInvitation
{
    public sealed class AcceptInvitationCommandHandler(
        IUserContext userContext,
        ITeamInvitationRepository invitationRepository,
        ITeamRepository teamRepository,
        IUnitOfWork uow) : IRequestHandler<AcceptInvitationCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            if (await invitationRepository.FindByIdAsync(request.InvitationId, cancellationToken) is not TeamInvitation invitation)
                return InvitationErrors.NotFound;

            var accepted = invitation.Accept(userContext.Email, DateTime.UtcNow);

            if (accepted.IsFailure)
                return accepted.Error;

            if (await teamRepository.FindByIdAsync(invitation.TeamId, cancellationToken) is not Team team)
                return InvitationErrors.NotFound;

            var joined = invitation.Role is TeamMemberRole.Administrator
                ? team.TryAddAdmin(userContext.Id)
                : team.TryAddMember(userContext.Id);

            if (joined.IsFailure)
                return joined.Error;

            try
            {
                await uow.CommitChangesAsync(cancellationToken);
            }
            catch (DuplicateEntryException)
            {
                return InvitationErrors.AlreadyMember;
            }

            return team.Id;
        }
    }
}
