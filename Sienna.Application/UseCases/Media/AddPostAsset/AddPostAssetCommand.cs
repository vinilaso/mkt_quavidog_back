using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.AddPostAsset
{
    /// <summary>
    /// Cadastra uma mídia e a associa à postagem na posição informada.
    /// Retorna o ID da mídia criada.
    /// </summary>
    public record AddPostAssetCommand(
        Guid PostId,
        string FileName,
        string Extension,
        Stream Content,
        int SequenceOrder) : ICommand<Result<Guid>>;
}
