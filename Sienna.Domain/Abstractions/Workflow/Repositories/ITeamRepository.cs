using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.Repositories
{
    public interface ITeamRepository : IAbstractRepository<Team>
    {
        Task<TeamCampaignsDTO?> GetTeamCampaignsAsync(Guid teamId, CancellationToken cancellationToken = default);
        Task<TeamAccessDTO?> GetAccessAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default);
        Task<TeamMembersDTO?> GetTeamMembersAsync(Guid teamId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UserContactDTO>> GetManagerContactsAsync(Guid teamId, CancellationToken cancellationToken = default);
    }
}
