using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Social.Instagram;
using Sienna.Application.UseCases.Social.Instagram.ConfigureAccount;
using Sienna.Application.UseCases.Social.Instagram.DeleteTeamAccount;
using Sienna.Application.UseCases.Social.Instagram.GetTeamAccount;
using Sienna.Application.UseCases.Social.Instagram.RequestPublication;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Social.Instagram;
using Sienna.WebApi.Extensions;
using Sienna.WebApi.OpenApi;
using System.ComponentModel;

namespace Sienna.WebApi.Endpoints
{
    public static class SocialEndpoints
    {
        public static IEndpointRouteBuilder MapSocialEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/teams/{teamId:guid}/social/instagram")
                .WithTags(ApiTags.Social)
                .RequireAuthorization();

            group
                .MapPut(string.Empty, async ([FromRoute, Description("ID do time.")] Guid teamId, [FromBody] ConfigureInstagramAccountRequest request, IMediator mediator) =>
                {
                    var command = new ConfigureInstagramAccountCommand(teamId, request.AccessToken);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .WithName("ConfigureInstagramAccount")
                .WithSummary("Conectar conta do Instagram")
                .ProducesWithDescription<InstagramAccountResponse>(StatusCodes.Status200OK, "O token foi validado na Meta e salvo para o time. Retorna a conta conectada.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "O token não foi informado, foi recusado pela Meta ou pertence a uma conta que não é Business/Creator.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .ProducesProblemWithDescription(StatusCodes.Status502BadGateway, "Não foi possível se comunicar com a API da Meta. Tente novamente em instantes.")
                .WithDescription("Conecta (ou substitui) a conta do Instagram do time a partir de um token de acesso gerado no painel da Meta. O ID e o nome da conta são obtidos automaticamente pelo token, que é armazenado cifrado e nunca é retornado pela API. Apenas o dono e os administradores do time podem configurar.");

            group
                .MapGet(string.Empty, async ([FromRoute, Description("ID do time.")] Guid teamId, IMediator mediator) =>
                {
                    var query = new GetTeamInstagramAccountQuery(teamId);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .WithName("GetInstagramAccount")
                .WithSummary("Consultar conta do Instagram")
                .ProducesWithDescription<InstagramConnectionResponse>(StatusCodes.Status200OK, "Situação da integração do time com o Instagram. Quando isConfigured é false, account vem nulo.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .WithDescription("Consulta se o time possui uma conta do Instagram conectada e, se possuir, quais são os dados dela. O token de acesso nunca é retornado. Qualquer membro do time pode consultar.");

            group
                .MapDelete(string.Empty, async ([FromRoute, Description("ID do time.")] Guid teamId, IMediator mediator) =>
                {
                    var command = new DeleteTeamAccountCommand(teamId);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.NoContent();
                })
                .WithName("DisconnectInstagramAccount")
                .WithSummary("Desconectar conta do Instagram")
                .ProducesWithDescription(StatusCodes.Status204NoContent, "A conta do Instagram foi desconectada do time.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O time não existe ou não possui uma conta do Instagram conectada.")
                .WithDescription("Desconecta a conta do Instagram do time, excluindo o token armazenado. Publicações já feitas no Instagram não são afetadas. Apenas o dono e os administradores do time podem desconectar.");

            group
                .MapPost("publications", async ([FromRoute, Description("ID do time.")] Guid teamId, [FromBody] RequestPublicationRequest request, IMediator mediator) =>
                {
                    var command = new RequestPublicationCommand(teamId, request.PostId, request.Format, request.ScheduledFor?.UtcDateTime);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Created($"/api/teams/{teamId}/social/instagram/publications/{result.Value.Id}", result.Value);
                })
                .WithName("RequestPublication")
                .WithSummary("Solicitar publicação no Instagram")
                .ProducesWithDescription<PublicationResponse>(StatusCodes.Status201Created, "A publicação foi registrada. Se quem solicitou não é gestor do time, ela aguarda aprovação (status pendingApproval).")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "O time não tem conta do Instagram conectada, a quantidade de imagens não é aceita no formato, alguma imagem não é JPEG ou o horário é inválido (no passado ou a mais de 30 dias).")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time, ou não é o autor da postagem nem gestor do time.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O time não existe, ou a postagem não existe ou não está em uma campanha do time.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "A postagem já possui uma publicação pendente, agendada ou publicada neste formato.")
                .WithDescription("Solicita a publicação de uma postagem no Instagram do time, como feed (1 imagem ou carrossel de até 10) ou stories (um por imagem, até 10). Sem scheduledFor, é publicada assim que possível. Solicitações de membros passam por aprovação de um dono ou administrador.");

            return builder;
        }
    }
}
