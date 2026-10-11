using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.CancelPublication
{
    public sealed record CancelPublicationCommand(Guid TeamId, Guid PublicationId) : ITeamScopedRequest, ICommand<Result<PublicationResponse>>;
}
