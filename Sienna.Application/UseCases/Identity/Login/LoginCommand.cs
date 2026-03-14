using MediatR;

namespace Sienna.Application.UseCases.Identity.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<string>;
}
