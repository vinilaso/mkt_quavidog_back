using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.GetUserPosts
{
    public record GetUserPostsQuery(Guid UserId) : IQuery<Result<GetUserPostsResponse>>;
}
