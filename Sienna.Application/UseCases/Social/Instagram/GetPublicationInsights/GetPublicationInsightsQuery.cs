using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.DTOs;

namespace Sienna.Application.UseCases.Social.Instagram.GetPublicationInsights
{
    public sealed record GetPublicationInsightsQuery(Guid TeamId, Guid PublicationId)
        : ITeamScopedRequest, IQuery<Result<PublicationInsightsDTO>>;
}
