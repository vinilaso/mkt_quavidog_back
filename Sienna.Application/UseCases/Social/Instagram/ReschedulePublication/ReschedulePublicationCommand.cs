using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.ReschedulePublication
{
    public sealed record ReschedulePublicationCommand(Guid TeamId, Guid PublicationId, DateTime ScheduledFor) 
        : ITeamScopedRequest, ICommand<Result<PublicationResponse>>;
}
