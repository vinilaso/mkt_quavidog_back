using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.ResetPassword
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Result>;
}
