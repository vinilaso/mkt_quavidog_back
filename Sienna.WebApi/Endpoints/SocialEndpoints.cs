using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Social.Instagram;
using Sienna.Application.UseCases.Social.Instagram.ConfigureAccount;
using Sienna.Application.UseCases.Social.Instagram.DeleteTeamAccount;
using Sienna.Application.UseCases.Social.Instagram.GetTeamAccount;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Social.Instagram;
using Sienna.WebApi.Extensions;

namespace Sienna.WebApi.Endpoints
{
    public static class SocialEndpoints
    {
        public static IEndpointRouteBuilder MapSocialEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/teams/{teamId:guid}/social/instagram")
                .WithTags("Social")
                .RequireAuthorization();

            group
                .MapPut(string.Empty, async ([FromRoute] Guid teamId, [FromBody] ConfigureInstagramAccountRequest request, IMediator mediator) =>
                {
                    var command = new ConfigureInstagramAccountCommand(teamId, request.AccessToken);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .ProducesWithDescription<InstagramAccountResponse>(StatusCodes.Status200OK, "O token foi validado na Meta e salvo para o time. Retorna a conta conectada.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "O token não foi informado, foi recusado pela Meta ou pertence a uma conta que não é Business/Creator.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .ProducesProblemWithDescription(StatusCodes.Status502BadGateway, "Não foi possível se comunicar com a API da Meta. Tente novamente em instantes.")
                .WithDescription("Conecta (ou substitui) a conta do Instagram do time a partir de um token de acesso gerado no painel da Meta. O ID e o nome da conta são obtidos automaticamente pelo token, que é armazenado cifrado e nunca é retornado pela API. Apenas o dono e os administradores do time podem configurar.");

            group
                .MapGet(string.Empty, async ([FromRoute]Guid teamId, IMediator mediator) =>
                {
                    var query = new GetTeamInstagramAccountQuery(teamId);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .ProducesWithDescription<InstagramConnectionResponse>(StatusCodes.Status200OK, "Situação da integração do time com o Instagram. Quando isConfigured é false, account vem nulo.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .WithDescription("Consulta se o time possui uma conta do Instagram conectada e, se possuir, quais são os dados dela. O token de acesso nunca é retornado. Qualquer membro do time pode consultar.");

            group
                .MapDelete(string.Empty, async ([FromRoute] Guid teamId, IMediator mediator) =>
                {
                    var command = new DeleteTeamAccountCommand(teamId);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.NoContent();
                })
                .ProducesWithDescription(StatusCodes.Status204NoContent, "A conta do Instagram foi desconectada do time.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O time não existe ou não possui uma conta do Instagram conectada.")
                .WithDescription("Desconecta a conta do Instagram do time, excluindo o token armazenado. Publicações já feitas no Instagram não são afetadas. Apenas o dono e os administradores do time podem desconectar.");

            return builder;
        }
    }
}
