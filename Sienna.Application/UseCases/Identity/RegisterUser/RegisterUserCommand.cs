using MediatR;
using Sienna.Domain.Abstractions;

namespace Sienna.Application.UseCases.Identity.RegisterUser
{
    public record RegisterUserCommand(
        string Email,
        string Password,
        string FullName
    ) : IRequest<Result<Guid>>;
}
