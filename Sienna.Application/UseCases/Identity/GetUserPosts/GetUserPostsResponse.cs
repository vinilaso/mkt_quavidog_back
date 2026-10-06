using Sienna.Domain.Abstractions.Media.DTOs;

namespace Sienna.Application.UseCases.Identity.GetUserPosts
{
    /// <summary>Postagens criadas por um usuário.</summary>
    /// <param name="UserId">ID do usuário.</param>
    /// <param name="Posts">Postagens do usuário, com suas imagens.</param>
    public record GetUserPostsResponse(Guid UserId, IEnumerable<PostDTO> Posts);
}
