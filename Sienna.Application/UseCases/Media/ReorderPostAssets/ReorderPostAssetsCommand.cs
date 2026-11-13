using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.ReorderPostAssets
{
    /// <summary>
    /// Define a nova ordem das imagens da postagem. A primeira mídia da lista fica na posição 1.
    /// </summary>
    public record ReorderPostAssetsCommand(Guid PostId, IReadOnlyList<Guid> MediaIds) : ICommand<Result>;
}
