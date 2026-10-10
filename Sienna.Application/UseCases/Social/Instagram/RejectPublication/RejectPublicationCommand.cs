using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.RejectPublication
{
    public sealed record RejectPublicationCommand(Guid TeamId, Guid PublicationId, string Reason) : ITeamManagerRequest, ICommand<Result<PublicationResponse>>;
}
