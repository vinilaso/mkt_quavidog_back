using Sienna.Application.UseCases.Identity.Login;

namespace Sienna.WebApi.Endpoints.Models.Identity
{
    /// <summary>Credenciais para autenticação.</summary>
    /// <param name="Email">E-mail cadastrado.</param>
    /// <param name="Password">Senha do usuário. Após 5 tentativas erradas, o usuário é bloqueado por 5 minutos.</param>
    public record LoginRequest(string Email, string Password)
    {
        public LoginCommand ToCommand() => new(Email, Password);
    }
}
