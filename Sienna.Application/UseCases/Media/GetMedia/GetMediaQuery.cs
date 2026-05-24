using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.GetMedia
{
    public record GetMediaQuery(Guid MediaId) : IQuery<Result<MediaResponse>>;
}
