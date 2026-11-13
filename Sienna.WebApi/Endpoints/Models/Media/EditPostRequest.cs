using Sienna.Application.UseCases.Media.EditPost;

namespace Sienna.WebApi.Endpoints.Models.Media
{
    /// <summary>Dados para editar uma postagem.</summary>
    /// <param name="Caption">Nova legenda da postagem. No Instagram, o limite é de 2.200 caracteres, 30 hashtags e 20 menções.</param>
    public record EditPostRequest(string Caption)
    {
        public EditPostCommand ToCommand(Guid postId) => new(postId, Caption);
    }
}
