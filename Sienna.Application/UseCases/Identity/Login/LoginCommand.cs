using MediatR;
using Sienna.Domain.Abstractions;

namespace Sienna.Application.UseCases.Identity.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<string>>;
}
