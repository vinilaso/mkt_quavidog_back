using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.GetCampaignInsights
{
    public sealed record GetCampaignInsightsQuery(Guid TeamId, Guid CampaignId)
        : ITeamScopedRequest, IQuery<Result<GetCampaignInsightsResponse>>;
}
