using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.ConfigureAccount
{
    public record ConfigureInstagramAccountCommand(Guid TeamId, string AccessToken) : ICommand<Result<InstagramAccountResponse>>, ITeamManagerRequest;
}
