using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.ApprovePublication
{
    public sealed record ApprovePublicationCommand(Guid TeamId, Guid PublicationId) : ITeamManagerRequest, ICommand<Result<PublicationResponse>>;
}
