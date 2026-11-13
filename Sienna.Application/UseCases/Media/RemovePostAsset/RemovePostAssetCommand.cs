using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.RemovePostAsset
{
    public record RemovePostAssetCommand(Guid PostId, Guid MediaId) : ICommand<Result>;
}
