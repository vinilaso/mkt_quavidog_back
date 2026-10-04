using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.RequestPublication
{
    public record RequestPublicationCommand(
         Guid TeamId,
         Guid PostId,
         PublicationFormat Format,
         DateTime? ScheduledFor) : ICommand<Result<PublicationResponse>>, ITeamScopedRequest;
}
