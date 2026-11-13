using Sienna.Domain.Abstractions.Media.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Media;

namespace Sienna.Application.UseCases.Media
{
    /// <summary>
    /// Carrega uma postagem garantindo que o usuário autenticado é o autor e que o conteúdo
    /// ainda pode ser alterado (nenhuma publicação aprovada, em andamento ou publicada).
    /// </summary>
    internal static class EditablePostLoader
    {
        internal static async Task<Result<Post>> LoadAsync(
            Guid postId,
            IUserContext userContext,
            IPostRepository postRepository,
            IPostPublicationRepository publicationRepository,
            CancellationToken cancellationToken)
        {
            if (!userContext.IsAuthenticated)
                return Error.Unauthorized("User.Unauthorized", "É necessário estar autenticado para editar uma postagem.");

            if (await postRepository.FindByIdAsync(postId, cancellationToken) is not Post post)
                return Error.NotFound("Post.NotFound", $"Não foi encontrada uma postagem no servidor com o ID {postId}.");

            if (post.AuthorId != userContext.Id)
                return Error.Forbidden("Post.NotAuthor", "Apenas o autor da postagem pode editá-la.");

            if (await publicationRepository.HasContentLockingPublicationAsync(post.Id, cancellationToken))
                return Error.Conflict("Post.Locked", "A postagem possui uma publicação aprovada, em andamento ou já publicada e não pode mais ser editada.");

            return post;
        }
    }
}
