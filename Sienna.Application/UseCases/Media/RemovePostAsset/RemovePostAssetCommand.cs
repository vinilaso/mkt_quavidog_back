using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.RemovePostAsset
{
    /// <summary>
    /// Remove uma imagem da postagem e renumera as restantes.
    /// </summary>
    public record RemovePostAssetCommand(Guid PostId, Guid MediaId) : ICommand<Result>;
}
