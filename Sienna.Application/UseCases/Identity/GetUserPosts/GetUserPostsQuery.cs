using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Media.DTOs;
using Sienna.Domain.Abstractions.Pagination;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.GetUserPosts
{
    public record GetUserPostsQuery(Guid UserId, PageRequest Page) : IQuery<Result<PagedResult<PostDTO>>>;
}
