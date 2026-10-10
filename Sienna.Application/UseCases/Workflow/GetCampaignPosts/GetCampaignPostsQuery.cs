using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns;

namespace Sienna.Application.UseCases.Workflow.GetCampaignPosts
{
    public record GetCampaignPostsQuery(Guid TeamId, Guid CampaignId) : ITeamScopedRequest, IQuery<Result<CampaignPostsDTO>>;
}
