using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.GetUserTeams
{
    public record GetUserTeamsQuery(Guid UserId) : IQuery<Result<UserTeamsDTO>>;
}
