using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow.DTOs.Invitations;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Shared.Rules;

namespace Sienna.Application.UseCases.Workflow.GetMyInvitations
{
    public sealed class GetMyInvitationsQueryHandler(
        IUserContext userContext,
        ITeamInvitationRepository invitationRepository) : IRequestHandler<GetMyInvitationsQuery, Result<IReadOnlyList<UserInvitationDTO>>>
    {
        public async Task<Result<IReadOnlyList<UserInvitationDTO>>> Handle(GetMyInvitationsQuery request, CancellationToken cancellationToken)
        {
            var normalizedEmail = EmailRules.Normalize(userContext.Email);
            var invitations = await invitationRepository.FindPendingByEmailAsync(normalizedEmail, DateTime.UtcNow, cancellationToken);

            return Result.Success(invitations);
        }
    }
}
