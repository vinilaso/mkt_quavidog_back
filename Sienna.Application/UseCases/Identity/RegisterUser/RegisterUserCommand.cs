using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.RegisterUser
{
    /// <summary>Dados para cadastrar um usuário.</summary>
    /// <param name="Email">E-mail do usuário. É também o login e precisa ser único.</param>
    /// <param name="Password">Senha com no mínimo 6 caracteres, contendo letra maiúscula, letra minúscula, número e símbolo.</param>
    /// <param name="FullName">Nome completo do usuário.</param>
    public record RegisterUserCommand(
        string Email,
        string Password,
        string FullName
    ) : IRequest<Result<Guid>>;
}
