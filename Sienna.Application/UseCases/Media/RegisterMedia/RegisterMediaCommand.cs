using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.RegisterMedia
{
    public record RegisterMediaCommand(string Name, string Extension, Stream Content) : ICommand<Result<Guid>>;
}
