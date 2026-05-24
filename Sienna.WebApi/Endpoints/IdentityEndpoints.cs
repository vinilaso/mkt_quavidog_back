using MediatR;
using Sienna.Application.UseCases.Identity.GetUserPosts;
using Sienna.Application.UseCases.Identity.GetUserProfile;
using Sienna.Application.UseCases.Identity.GetUserTeams;
using Sienna.Application.UseCases.Identity.Login;
using Sienna.Application.UseCases.Identity.RegisterUser;
using Sienna.Application.UseCases.Identity.ResetPassword;
using Sienna.Application.UseCases.Identity.ResetPassword.SendToken;
using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Security;
using Sienna.WebApi.Contracts.Identity;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Extensions;

namespace Sienna.WebApi.Endpoints
{
    public static class IdentityEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/identity").WithTags("Identity");

            group
                .MapPost("users", EndpointBodyFactory.Create<RegisterUserCommand, Guid>(guid => TypedResults.Created("users/me", guid)))
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "O usuário foi criado com sucesso no banco de dados.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "Falha de validação nos parâmetros de entrada.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "O e-mail já foi registrado anteriormente.")
                .WithDescription("Cria um usuário.");

            group
                .MapGet("users/me", async (IUserContext userContext, IMediator mediator) =>
                {
                    if (!userContext.IsAuthenticated)
                        return TypedResults.Unauthorized();

                    var query = new GetUserProfileQuery(userContext.Id);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .ProducesWithDescription<UserProfileResponse>(StatusCodes.Status200OK, "As informações do usuário autenticado foram encontradas e retornadas.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O ID do usuário autenticado não foi encontrado no servidor.")
                .WithDescription("Busca as informações do usuário autenticado.")
                .RequireAuthorization();

            group
                .MapGet("users/me/teams", async (IUserContext userContext, IMediator mediator) =>
                {
                    if (!userContext.IsAuthenticated)
                        return TypedResults.Unauthorized();

                    var query = new GetUserTeamsQuery(userContext.Id);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .ProducesWithDescription<UserTeamsDTO>(StatusCodes.Status200OK, "Os times do usuário autenticado foram encontrados e retornados.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O ID do usuário autenticado não foi encontrado no servidor.")
                .WithDescription("Busca os times do usuário autenticado.")
                .RequireAuthorization();

            group
                .MapGet("users/me/posts", async (IUserContext userContext, IMediator mediator) =>
                {
                    if (!userContext.IsAuthenticated)
                        return TypedResults.Unauthorized();

                    var query = new GetUserPostsQuery(userContext.Id);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                });

            group
                .MapPost("users/reset-password/confirm", EndpointBodyFactory.Create<ResetPasswordCommand>(TypedResults.Ok))
                .ProducesWithDescription(StatusCodes.Status200OK, "A senha foi alterada com sucesso.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado usuário com o e-mail informado.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "Os parâmetros de entrada estavam inválidos.")
                .WithDescription("Altera a senha de um usuário utilizando um token de redefinição de senha.");

            group
                .MapPost("tokens/login", EndpointBodyFactory.Create<LoginCommand, string>(token => TypedResults.Ok(new LoginResponse(token))))
                .ProducesWithDescription<LoginResponse>(StatusCodes.Status200OK, "O usuário com e-mail e senha informados foi encontrado e o token JWT foi gerado.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "Não foi encontrado usuário com e-mail e senha informados, ou o usuário está bloqueado.")
                .WithDescription("Gera um token JWT que pode ser utilizado como forma de autenticação.");

            group
                .MapPost("tokens/reset-password", EndpointBodyFactory.Create<SendPassowordResetTokenCommand>(TypedResults.Ok))
                .ProducesWithDescription(StatusCodes.Status200OK, "O email com o token de redefinição de senha foi enviado com sucesso.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado usuário com o e-mail informado.")
                .ProducesProblemWithDescription(StatusCodes.Status500InternalServerError, "Não foi possível gerar o token de redefinição de senha por algum motivo.")
                .WithDescription("Envia um e-mail para o usuário com seu token de redefinição de senha.");

            return builder;
        }
    }
}
