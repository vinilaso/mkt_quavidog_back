using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.ResetPassword
{
    /// <summary>Dados para redefinir a senha.</summary>
    /// <param name="Email">E-mail do usuário.</param>
    /// <param name="Token">Token de redefinição recebido por e-mail.</param>
    /// <param name="NewPassword">Nova senha, com as mesmas regras do cadastro.</param>
    public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest<Result>;
}
