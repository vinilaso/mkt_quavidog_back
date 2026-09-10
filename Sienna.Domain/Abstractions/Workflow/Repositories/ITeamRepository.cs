using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.Repositories
{
    public interface ITeamRepository : IAbstractRepository<Team>
    {
        Task<TeamCampaignsDTO?> GetTeamCampaignsAsync(Guid teamId, CancellationToken cancellationToken = default);
    }
}
