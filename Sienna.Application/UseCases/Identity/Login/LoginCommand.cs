using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.Login
{
    /// <summary>Credenciais para autenticação.</summary>
    /// <param name="Email">E-mail cadastrado.</param>
    /// <param name="Password">Senha do usuário. Após 5 tentativas erradas, o usuário é bloqueado por 5 minutos.</param>
    public record LoginCommand(string Email, string Password) : IRequest<Result<string>>;
}
