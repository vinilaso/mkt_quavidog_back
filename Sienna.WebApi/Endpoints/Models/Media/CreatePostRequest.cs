using Sienna.Application.UseCases.Media.RegisterPost;

namespace Sienna.WebApi.Endpoints.Models.Media
{
    /// <summary>Dados para cadastrar uma postagem.</summary>
    /// <param name="Caption">Legenda da postagem. No Instagram, o limite é de 2.200 caracteres, 30 hashtags e 20 menções.</param>
    public record CreatePostRequest(string Caption)
    {
        public RegisterPostCommand ToCommand() => new(Caption);
    }
}
