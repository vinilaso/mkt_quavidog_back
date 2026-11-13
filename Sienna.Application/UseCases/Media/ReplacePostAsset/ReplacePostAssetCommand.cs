using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.ReplacePostAsset
{
    public record ReplacePostAssetCommand(
        Guid PostId,
        Guid MediaId,
        string FileName,
        string Extension,
        Stream Content) : ICommand<Result>;
}
