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

        public async Task<TeamAccessDTO?> GetAccessAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Team>()
                .Where(team => team.Id == teamId)
                .Select(team => new TeamAccessDTO
                {
                    TeamId = team.Id,
                    Role = team.Members
                        .Where(member => member.MemberId == userId)
                        .Select(member => (TeamMemberRole?)member.Role)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public override async Task<Team?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Team>()
                .Include(team => team.Members)
                .Where(team => team.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TeamMembersDTO?> GetTeamMembersAsync(Guid teamId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<Team>()
                .Where(team => team.Id == teamId)
                .Select(team => new TeamMembersDTO
                {
                    TeamId = team.Id,
                    TeamName = team.Name,
                    Members = team.Members
                        .OrderBy(member => member.Role)
                        .ThenBy(member => member.Member!.FullName)
                        .Select(member => new TeamMemberDTO
                        {
                            UserId = member.MemberId.GetValueOrDefault(),
                            UserName = member.Member!.FullName,
                            Role = member.Role.ToString().ToLowerInvariant(),
                            JoinedAt = member.AssociationDate,
                            UserEmail = member.Member.Email!
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
