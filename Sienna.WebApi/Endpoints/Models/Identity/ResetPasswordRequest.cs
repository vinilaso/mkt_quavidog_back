using Sienna.Application.UseCases.Identity.ResetPassword;

namespace Sienna.WebApi.Endpoints.Models.Identity
{
    /// <summary>Dados para redefinir a senha.</summary>
    /// <param name="Email">E-mail do usuário.</param>
    /// <param name="Token">Token de redefinição recebido por e-mail.</param>
    /// <param name="NewPassword">Nova senha, com as mesmas regras do cadastro.</param>
    public record ResetPasswordRequest(string Email, string Token, string NewPassword)
    {
        public ResetPasswordCommand ToCommand() => new(Email, Token, NewPassword);
    }
}
