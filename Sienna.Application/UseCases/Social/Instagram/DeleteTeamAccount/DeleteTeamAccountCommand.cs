using Sienna.Application.Messaging;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.DeleteTeamAccount
{
    public record DeleteTeamAccountCommand(Guid TeamId) : ITeamManagerRequest, ICommand<Result>;
}
