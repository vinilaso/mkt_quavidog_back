using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.AddPostAsset
{
    public record AddPostAssetCommand(Guid PostId, Guid MediaId, int SequenceOrder) : ICommand<Result>;
}
