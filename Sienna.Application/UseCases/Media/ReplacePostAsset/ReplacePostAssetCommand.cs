using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.ReplacePostAsset
{
    /// <summary>
    /// Substitui o arquivo de uma imagem da postagem, mantendo o ID da mídia e a posição.
    /// </summary>
    public record ReplacePostAssetCommand(
        Guid PostId,
        Guid MediaId,
        string FileName,
        string Extension,
        Stream Content) : ICommand<Result>;
}
