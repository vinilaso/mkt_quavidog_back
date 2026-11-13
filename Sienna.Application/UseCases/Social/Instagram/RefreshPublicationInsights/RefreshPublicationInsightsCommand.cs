using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.DTOs;

namespace Sienna.Application.UseCases.Social.Instagram.RefreshPublicationInsights
{
    public sealed record RefreshPublicationInsightsCommand(Guid TeamId, Guid PublicationId)
        : ITeamScopedRequest, ICommand<Result<PublicationInsightsDTO>>;
}
