using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Invitations;
using Sienna.Domain.Abstractions.Workflow.Repositories;

namespace Sienna.Application.UseCases.Workflow.GetTeamInvitations
{
    public sealed class GetTeamInvitationQueryHandler(ITeamInvitationRepository invitationRepository) : IRequestHandler<GetTeamInvitationsQuery, Result<IReadOnlyList<TeamInvitationDTO>>>
    {
        public async Task<Result<IReadOnlyList<TeamInvitationDTO>>> Handle(GetTeamInvitationsQuery request, CancellationToken cancellationToken)
        {
            var invitations = await invitationRepository.GetPendingByTeamAsync(request.TeamId, DateTime.UtcNow, cancellationToken);
            return Result.Success(invitations);
        }
    }
}
