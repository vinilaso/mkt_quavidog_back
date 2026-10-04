using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.GetTeamAccount
{
    public record GetTeamInstagramAccountQuery(Guid TeamId) : IQuery<Result<InstagramConnectionResponse>>, ITeamScopedRequest;
}
