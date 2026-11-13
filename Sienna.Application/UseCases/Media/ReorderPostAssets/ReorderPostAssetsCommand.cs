using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.ReorderPostAssets
{
    public record ReorderPostAssetsCommand(Guid PostId, IReadOnlyList<Guid> MediaIds) : ICommand<Result>;
}
