using Sienna.Application.UseCases.Identity.ResetPassword.SendToken;

namespace Sienna.WebApi.Endpoints.Models.Identity
{
    /// <summary>Dados para solicitar a redefinição de senha.</summary>
    /// <param name="Email">E-mail do usuário que receberá o token de redefinição.</param>
    public record SendPasswordResetTokenRequest(string Email)
    {
        public SendPassowordResetTokenCommand ToCommand() => new(Email);
    }
}
