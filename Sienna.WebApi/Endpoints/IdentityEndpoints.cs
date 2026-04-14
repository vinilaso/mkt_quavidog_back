using Sienna.Application.UseCases.Identity.Login;
using Sienna.Application.UseCases.Identity.RegisterUser;
using Sienna.WebApi.Contracts.Identity;
using Sienna.WebApi.Endpoints.Extensions;

namespace Sienna.WebApi.Endpoints
{
    public static class IdentityEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/identity").WithTags("Identity");

            group
                .MapPost("users", EndpointBodyFactory.Create<RegisterUserCommand, Guid>(guid => TypedResults.Created(string.Empty, guid)))
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "O usuário foi criado com sucesso no banco de dados.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "Falha de validação nos parâmetros de entrada.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "O e-mail já foi registrado anteriormente.");

            group
                .MapPost("tokens/login", EndpointBodyFactory.Create<LoginCommand, string>(token => TypedResults.Ok(new LoginResponse(token))))
                .ProducesWithDescription<LoginResponse>(StatusCodes.Status200OK, "O usuário com e-mail e senha informados foi encontrado e o token JWT foi gerado.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "Não foi encontrado usuário com e-mail e senha informados, ou o usuário está bloqueado.");

            return builder;
        }
    }
}
