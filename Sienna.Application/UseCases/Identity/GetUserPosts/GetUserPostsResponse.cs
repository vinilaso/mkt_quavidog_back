using Sienna.Domain.Abstractions.Media.DTOs;

namespace Sienna.Application.UseCases.Identity.GetUserPosts
{
    public record GetUserPostsResponse(Guid UserId, IEnumerable<PostDTO> Posts);
}
