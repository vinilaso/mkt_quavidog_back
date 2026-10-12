using Sienna.Domain.Abstractions.Workflow.DTOs.Invitations;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.Repositories
{
    public interface ITeamInvitationRepository : IAbstractRepository<TeamInvitation>
    {
        Task<TeamInvitation?> FindPendingAsync(Guid teamId, string normalizedEmail, CancellationToken cancellationToken = default);
        Task<TeamInvitation?> FindTeamInvitationAsync(Guid teamId, Guid invitationId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TeamInvitationDTO>> GetPendingByTeamAsync(Guid teamId, DateTime utcNow, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UserInvitationDTO>> FindPendingByEmailAsync(string normalizedEmail, DateTime utcNow, CancellationToken cancellationToken = default);
        Task<InvitationEmailDTO?> GetEmailDataAsync(Guid invitationId, CancellationToken cancellationToken = default);
    }
}
