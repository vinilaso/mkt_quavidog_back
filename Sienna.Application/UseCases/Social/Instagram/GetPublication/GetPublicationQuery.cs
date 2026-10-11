using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.DTOs;

namespace Sienna.Application.UseCases.Social.Instagram.GetPublication
{
    public sealed record GetPublicationQuery(Guid TeamId, Guid PublicationId) 
        : ITeamScopedRequest, IQuery<Result<PublicationDetailsDTO>>;
}
