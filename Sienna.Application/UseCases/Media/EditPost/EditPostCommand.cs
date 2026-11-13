using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Media.EditPost
{
    public record EditPostCommand(Guid PostId, string Caption) : ICommand<Result>;
}
