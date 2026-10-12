using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Workflow.CancelInvitation;
using Sienna.Application.UseCases.Workflow.GetCampaignPosts;
using Sienna.Application.UseCases.Workflow.GetTeamCampaigns;
using Sienna.Application.UseCases.Workflow.GetTeamInvitations;
using Sienna.Application.UseCases.Workflow.GetTeamMembers;
using Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns;
using Sienna.Domain.Abstractions.Workflow.DTOs.Invitations;
using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Workflow.Campaigns;
using Sienna.WebApi.Endpoints.Models.Workflow.Teams;
using Sienna.WebApi.Extensions;
using Sienna.WebApi.OpenApi;
using Sienna.WebApi.RateLimiting;
using System.ComponentModel;

namespace Sienna.WebApi.Endpoints
{
    public class WorkflowEndpoints : IEndpointGroup
    {
        public void Map(IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/workflow").WithTags(ApiTags.Workflow).RequireAuthorization();

            group
                .MapPost("teams", CreateTeam)
                .WithName("CreateTeam")
                .WithSummary("Criar time")
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "O time foi criado com sucesso.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .WithDescription("Cria um time. O usuário logado é marcado como dono do time criado.");

            group
                .MapGet("teams/{teamId:guid}/members", GetTeamMembers)
                .WithName("GetTeamMembers")
                .WithSummary("Listar membros do time")
                .ProducesWithDescription<TeamMembersDTO>(StatusCodes.Status200OK, "Os membros do time foram encontrados e retornados.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .WithDescription("Lista os membros de um time, ordenados por papel (dono, administradores e membros) e depois por nome. O campo role vem como \"owner\", \"administrator\" ou \"member\". Qualquer membro do time pode consultar.");

            group
                .MapPost("teams/{teamId:guid}/invitations", InviteToTeam)
                .WithName("InviteToTeam")
                .WithSummary("Convidar para o time")
                .RequireRateLimiting(RateLimitPolicies.Invitations)
                .ProducesWithDescription(StatusCodes.Status202Accepted, "O convite foi registrado e o e-mail será enviado.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "O e-mail é inválido ou o papel não é member nem administrator.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "O e-mail já pertence a um membro do time.")
                .ProducesProblemWithDescription(StatusCodes.Status429TooManyRequests, "O limite de 20 convites por hora foi atingido.")
                .WithDescription("Convida um e-mail para o time. A resposta é a mesma exista ou não uma conta com esse e-mail. Convidar de novo um e-mail com convite pendente renova o convite por mais 7 dias. Apenas o dono e os administradores do time podem convidar.");

            group
                .MapGet("teams/{teamId:guid}/invitations", GetTeamInvitations)
                .WithName("GetTeamInvitations")
                .WithSummary("Listar convites pendentes do time")
                .ProducesWithDescription<IReadOnlyList<TeamInvitationDTO>>(StatusCodes.Status200OK, "Os convites pendentes e ainda válidos do time, dos mais recentes para os mais antigos.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .WithDescription("Lista os convites do time que ainda aguardam resposta, com o e-mail convidado, o papel oferecido, quem convidou e a validade. Convites expirados, aceitos, recusados ou cancelados não aparecem. Apenas o dono e os administradores do time podem consultar.");


            group
                .MapDelete("teams/{teamId:guid}/invitations/{invitationId:guid}", CancelInvitation)
                .WithName("CancelInvitation")
                .WithSummary("Cancelar convite")
                .ProducesWithDescription(StatusCodes.Status204NoContent, "O convite foi cancelado e não pode mais ser aceito.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O time não existe ou o convite não pertence ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "O convite já foi aceito, recusado ou cancelado.")
                .WithDescription("Cancela um convite pendente do time. Depois de cancelado, o convidado não consegue mais aceitá-lo, mas um novo convite pode ser enviado para o mesmo e-mail. Apenas o dono e os administradores do time podem cancelar.");

            group
                .MapPost("teams/{teamId:guid}/campaigns", CreateCampaign)
                .WithName("CreateCampaign")
                .WithSummary("Criar campanha")
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "A campanha foi criada com sucesso.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .WithDescription("Cria uma campanha ativa no time informado. Qualquer membro do time pode criar campanhas.");

            group
                .MapGet("teams/{teamId:guid}/campaigns", GetTeamCampaigns)
                .WithName("GetTeamCampaigns")
                .WithSummary("Listar campanhas do time")
                .ProducesWithDescription<TeamCampaignsDTO>(StatusCodes.Status200OK, "As campanhas do time foram encontradas e retornadas.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .WithDescription("Lista as campanhas de um time. Qualquer membro do time pode consultar.");

            group
                .MapPost("teams/{teamId:guid}/campaigns/{campaignId:guid}/posts", AddPostToCampaign)
                .WithName("AddPostToCampaign")
                .WithSummary("Adicionar postagem à campanha")
                .ProducesWithDescription(StatusCodes.Status200OK, "A postagem foi associada à campanha.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é o autor da postagem.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O time não existe, a campanha não pertence ao time ou a postagem não foi encontrada.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "A postagem já pertence à campanha.")
                .WithDescription("Associa uma postagem a uma campanha do time. Apenas o autor da postagem pode associá-la.");

            group
                .MapGet("teams/{teamId:guid}/campaigns/{campaignId:guid}/posts", GetCampaignPosts)
                .WithName("GetCampaignPosts")
                .WithSummary("Listar postagens da campanha")
                .ProducesWithDescription<CampaignPostsDTO>(StatusCodes.Status200OK, "As postagens da campanha foram encontradas e retornadas.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O time não existe ou a campanha não pertence ao time.")
                .WithDescription("Lista as postagens de uma campanha do time. Qualquer membro do time pode consultar.");
        }

        private static async Task<IResult> CreateTeam([FromBody] CreateTeamRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(), cancellationToken);

            return result.ToHttpResult(id => TypedResults.CreatedAtRoute(
                value: id,
                routeName: "GetCurrentUserTeams"
            ));
        }

        private static async Task<IResult> GetTeamMembers([FromRoute, Description("ID do time.")] Guid teamId, IMediator mediator, CancellationToken cancellationToken)
        {
            var query = new GetTeamMembersQuery(teamId);
            var result = await mediator.Send(query, cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> CreateCampaign([FromRoute, Description("ID do time.")] Guid teamId, [FromBody] CreateCampaignRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(teamId), cancellationToken);

            return result.ToHttpResult(id => TypedResults.CreatedAtRoute(
                value: id,
                routeName: "GetTeamCampaigns",
                routeValues: new { teamId }
            ));
        }

        private static async Task<IResult> GetTeamCampaigns([FromRoute, Description("ID do time.")] Guid teamId, IMediator mediator, CancellationToken cancellationToken)
        {
            var query = new GetTeamCampaignsQuery(teamId);
            var result = await mediator.Send(query, cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> AddPostToCampaign([FromRoute, Description("ID do time.")] Guid teamId, [FromRoute, Description("ID da campanha.")] Guid campaignId, [FromBody] AssignPostToCampaignRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(teamId, campaignId), cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> GetCampaignPosts([FromRoute, Description("ID do time.")] Guid teamId, [FromRoute, Description("ID da campanha.")] Guid campaignId, IMediator mediator, CancellationToken cancellationToken)
        {
            var query = new GetCampaignPostsQuery(teamId, campaignId);
            var result = await mediator.Send(query, cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> InviteToTeam(
            [FromRoute, Description("ID do time.")]Guid teamId,
            [FromBody]InviteToTeamRequest request,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(request.ToCommand(teamId), cancellationToken);
            return result.ToHttpResult(
                onSuccess: () => TypedResults.Accepted(default(string))
            );
        }

        private static async Task<IResult> GetTeamInvitations(
            [FromRoute, Description("ID do time.")]Guid teamId,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetTeamInvitationsQuery(teamId), cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
        }

        private static async Task<IResult> CancelInvitation(
            [FromRoute, Description("ID do time.")]Guid teamId,
            [FromRoute, Description("ID do convite.")]Guid invitationId,
            IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new CancelInvitationCommand(teamId, invitationId), cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.NoContent
            );
        }
    }
}
