using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.ResetPassword.SendToken
{
    /// <summary>Dados para solicitar a redefinição de senha.</summary>
    /// <param name="Email">E-mail do usuário que receberá o token de redefinição.</param>
    public record SendPassowordResetTokenCommand(string Email) : IRequest<Result>;
}
