using Sienna.Application.Messaging;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Social.Instagram.ExecutePublication
{
    public record ExecutePublicationCommand(Guid PublicationId) : ICommand<Result<ExecutePublicationResponse>>;
}
