using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.RefreshTeamInsights
{
    public sealed record RefreshTeamInsightsCommand(Guid TeamId)
        : ITeamScopedRequest, ICommand<Result<RefreshTeamInsightsResponse>>;
}
