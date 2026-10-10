using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Workflow.GetCampaignPosts;
using Sienna.Application.UseCases.Workflow.GetTeamCampaigns;
using Sienna.Application.UseCases.Workflow.GetTeamMembers;
using Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns;
using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Workflow.Campaigns;
using Sienna.WebApi.Endpoints.Models.Workflow.Teams;
using Sienna.WebApi.Extensions;
using Sienna.WebApi.OpenApi;
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
                .MapPost("teams/{teamId:guid}/members", AddTeamMember)
                .WithName("AddTeamMember")
                .WithSummary("Adicionar membro ao time")
                .ProducesWithDescription(StatusCodes.Status200OK, "O usuário foi adicionado ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, $"O papel informado não é \"member\" nem \"administrator\".")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O time ou o usuário a ser adicionado não foi encontrado.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "O usuário já pertence ao time.")
                .WithDescription($"Adiciona um usuário ao time. O campo role aceita \"member\" ou \"administrator\" (sem diferenciar maiúsculas). Apenas o dono e os administradores do time podem adicionar usuários.");

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

        private static async Task<IResult> AddTeamMember([FromRoute, Description("ID do time.")] Guid teamId, [FromBody] AssignUserToTeamRequest request, IMediator mediator, CancellationToken cancellationToken)
        {
            var command = request.ToCommand(teamId);

            if (command.IsFailure)
                return command.Error.CreateProblemDetails();

            var result = await mediator.Send(command.Value, cancellationToken);

            return result.ToHttpResult(
                onSuccess: TypedResults.Ok
            );
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
    }
}
