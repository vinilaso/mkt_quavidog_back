using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.RegisterPost
{
    public record RegisterPostCommand(string Caption) : ICommand<Result<Guid>>;
}
