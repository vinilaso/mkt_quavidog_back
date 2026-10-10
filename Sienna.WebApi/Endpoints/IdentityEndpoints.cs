using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Identity.GetUserPosts;
using Sienna.Application.UseCases.Identity.GetUserProfile;
using Sienna.Application.UseCases.Identity.GetUserTeams;
using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Media.DTOs;
using Sienna.Domain.Abstractions.Pagination;
using Sienna.Domain.Abstractions.Security;
using Sienna.WebApi.Contracts.Identity;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Identity;
using Sienna.WebApi.Endpoints.Models.Pagination;
using Sienna.WebApi.Extensions;
using Sienna.WebApi.OpenApi;

namespace Sienna.WebApi.Endpoints
{
    public sealed class IdentityEndpoints : IEndpointGroup
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/identity").WithTags(ApiTags.Identity);

            #region Rotas públicas

            group
                .MapPost("users", RegisterUser)
                .WithName("RegisterUser")
                .WithSummary("Cadastrar usuário")
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "O usuário foi criado com sucesso no banco de dados.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "Falha de validação nos parâmetros de entrada.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "O e-mail já foi registrado anteriormente.")
                .WithDescription("Cria um usuário.");

            group
                .MapPost("tokens/login", Login)
                .WithName("Login")
                .WithSummary("Autenticar usuário (login)")
                .ProducesWithDescription<LoginResponse>(StatusCodes.Status200OK, "O usuário com e-mail e senha informados foi encontrado e o token JWT foi gerado.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "Não foi encontrado usuário com e-mail e senha informados, ou o usuário está bloqueado.")
                .WithDescription("Gera um token JWT que pode ser utilizado como forma de autenticação.");

            group
                .MapPost("tokens/reset-password", SendPasswordResetToken)
                .WithName("SendPasswordResetToken")
                .WithSummary("Solicitar redefinição de senha")
                .ProducesWithDescription(StatusCodes.Status200OK, "O email com o token de redefinição de senha foi enviado com sucesso.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado usuário com o e-mail informado.")
                .ProducesProblemWithDescription(StatusCodes.Status500InternalServerError, "Não foi possível gerar o token de redefinição de senha por algum motivo.")
                .WithDescription("Envia um e-mail para o usuário com seu token de redefinição de senha.");

            group
                .MapPost("users/reset-password/confirm", ResetPassword)
                .WithName("ResetPassword")
                .WithSummary("Redefinir senha")
                .ProducesWithDescription(StatusCodes.Status200OK, "A senha foi alterada com sucesso.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado usuário com o e-mail informado.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "Os parâmetros de entrada estavam inválidos.")
                .WithDescription("Altera a senha de um usuário utilizando um token de redefinição de senha.");

            #endregion

            #region Rotas do usuário autenticado

            group
                .MapGet("users/me", GetCurrentUser)
                .WithName("GetCurrentUser")
                .WithSummary("Consultar perfil do usuário autenticado")
                .ProducesWithDescription<UserProfileResponse>(StatusCodes.Status200OK, "As informações do usuário autenticado foram encontradas e retornadas.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O ID do usuário autenticado não foi encontrado no servidor.")
                .WithDescription("Busca as informações do usuário autenticado.")
                .RequireAuthorization();

            group
                .MapGet("users/me/teams", GetCurrentUserTeams)
                .WithName("GetCurrentUserTeams")
                .WithSummary("Listar times do usuário autenticado")
                .ProducesWithDescription<UserTeamsDTO>(StatusCodes.Status200OK, "Os times do usuário autenticado foram encontrados e retornados.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O ID do usuário autenticado não foi encontrado no servidor.")
                .WithDescription("Busca os times do usuário autenticado.")
                .RequireAuthorization();

            group
                .MapGet("users/me/posts", GetCurrentUserPosts)
                .WithName("GetCurrentUserPosts")
                .WithSummary("Listar postagens do usuário autenticado")
                .ProducesWithDescription<PagedResult<PostDTO>>(StatusCodes.Status200OK, "Uma página das postagens do usuário autenticado, das mais recentes para as mais antigas.")
                .ProducesProblemWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .WithDescription("Busca as postagens criadas pelo usuário autenticado, com suas mídias.")
                .RequireAuthorization();

            #endregion
        }

        private static async Task<IResult> RegisterUser([FromBody]RegisterUserRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);

            return result.ToHttpResult(
                onSuccess: guid => TypedResults.Created("/api/identity/users/me", guid)
            );
        }

        private static async Task<IResult> Login([FromBody]LoginRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);
            return result.ToHttpResult(
                onSuccess: token => TypedResults.Ok(new LoginResponse(token))
            );
        }

        private static async Task<IResult> SendPasswordResetToken([FromBody]SendPasswordResetTokenRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> ResetPassword([FromBody]ResetPasswordRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> GetCurrentUser(IUserContext userContext, IMediator mediator, CancellationToken cancellationToken)
        {
            if (!userContext.IsAuthenticated)
                return TypedResults.Unauthorized();

            var query = new GetUserProfileQuery(userContext.Id);
            var result = await mediator.Send(query, cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> GetCurrentUserTeams(IUserContext userContext, IMediator mediator, CancellationToken cancellationToken)
        {
            if (!userContext.IsAuthenticated)
                return TypedResults.Unauthorized();

            var query = new GetUserTeamsQuery(userContext.Id);
            var result = await mediator.Send(query, cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> GetCurrentUserPosts([AsParameters]PaginationParameters pagination, IUserContext userContext, IMediator mediator, CancellationToken cancellationToken)
        {
            if (!userContext.IsAuthenticated)
                return TypedResults.Unauthorized();

            var query = new GetUserPostsQuery(userContext.Id, pagination.ToPageRequest());
            var result = await mediator.Send(query, cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }
    }
}
