using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Invitations;

namespace Sienna.Application.UseCases.Workflow.GetMyInvitations
{
    public sealed record GetMyInvitationsQuery : IQuery<Result<IReadOnlyList<UserInvitationDTO>>>;
}
