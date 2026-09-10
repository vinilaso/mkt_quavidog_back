using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Infrastructure.Repositories.Workflow
{
    internal class TeamRepository(ApplicationContext context) : AbstractRepository<Team>(context), ITeamRepository
    {
        public async Task<TeamCampaignsDTO?> GetTeamCampaignsAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Team>()
                .Where(team => team.Id == teamId)
                .Select(team => new TeamCampaignsDTO
                {
                    TeamId = team.Id,
                    Campaigns = team.Campaigns.Select(campaign => new TeamCampaignDTO
                    {
                        CampaignId = campaign.Id,
                        Name = campaign.Name,
                        Status = campaign.Status.ToString()
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
