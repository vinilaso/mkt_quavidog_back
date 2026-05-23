using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.GetUserProfile
{
    public record GetUserProfileQuery(Guid UserId) : IQuery<Result<UserProfileResponse>>;
}
