using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.ResetPassword.SendToken
{
    public record SendPassowordResetTokenCommand(string Email) : IRequest<Result>;
}
