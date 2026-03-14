using MediatR;

namespace Sienna.Application.UseCases.Identity.RegisterUser
{
    public record RegisterUserCommand(
        string Email,
        string Password,
        string FullName
    ) : IRequest<Guid>;
}
