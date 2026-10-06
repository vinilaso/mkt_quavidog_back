using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.RegisterPost
{
    /// <summary>Dados para cadastrar uma postagem.</summary>
    /// <param name="Caption">Legenda da postagem. No Instagram, o limite é de 2.200 caracteres, 30 hashtags e 20 menções.</param>
    public record RegisterPostCommand(string Caption) : ICommand<Result<Guid>>;
}
