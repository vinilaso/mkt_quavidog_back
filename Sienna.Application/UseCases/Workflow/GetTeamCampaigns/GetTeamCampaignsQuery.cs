using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;

namespace Sienna.Application.UseCases.Workflow.GetTeamCampaigns
{
    public record GetTeamCampaignsQuery(Guid TeamId) : IQuery<Result<TeamCampaignsDTO>>;
}
