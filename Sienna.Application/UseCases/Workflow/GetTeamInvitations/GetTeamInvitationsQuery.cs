using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Invitations;

namespace Sienna.Application.UseCases.Workflow.GetTeamInvitations
{
    public sealed record GetTeamInvitationsQuery(Guid TeamId) 
        : ITeamManagerRequest, IQuery<Result<IReadOnlyList<TeamInvitationDTO>>>;
}