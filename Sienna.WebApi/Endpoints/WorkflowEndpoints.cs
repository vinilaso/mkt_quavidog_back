using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Workflow.AssignPostToCampaign;
using Sienna.Application.UseCases.Workflow.AssignUserToTeam;
using Sienna.Application.UseCases.Workflow.CreateCampaign;
using Sienna.Application.UseCases.Workflow.CreateTeam;
using Sienna.Application.UseCases.Workflow.GetCampaignPosts;
using Sienna.Application.UseCases.Workflow.GetTeamCampaigns;
using Sienna.Application.UseCases.Workflow.GetTeamMembers;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns;
using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Workflow.Campaigns;
using Sienna.WebApi.Endpoints.Models.Workflow.Teams;
using Sienna.WebApi.Extensions;

namespace Sienna.WebApi.Endpoints
{
    public static class WorkflowEndpoints
    {
        public static IEndpointRouteBuilder MapWorkflowEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("api/workflow").WithTags("Workflow").RequireAuthorization();

            group
                .MapPost("teams", EndpointBodyFactory.Create<CreateTeamCommand, Guid>(guid => TypedResults.Created(string.Empty, guid)))
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "O time foi criado com sucesso.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .WithDescription("Cria um time. O usuário logado é marcado como dono do time criado.");

            group
                .MapPost("teams/{teamId:guid}/members", async ([FromRoute] Guid teamId, [FromBody] AssignUserToTeamRequest request, IMediator mediator) =>
                {
                    var role = request.ToTeamMemberRole();

                    if (role is null)
                        return Error.Validation("Team.InvalidRole", "O papel deve ser \"member\" ou \"administrator\".").CreateProblemDetails();

                    var command = new AssignUserToTeamCommand(teamId, request.UserId, role.Value);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok();
                })
                .ProducesWithDescription(StatusCodes.Status200OK, "O usuário foi adicionado ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status400BadRequest, "O papel informado não é \"member\" nem \"administrator\".")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time ou não é dono/administrador dele.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "O time ou o usuário a ser adicionado não foi encontrado.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "O usuário já pertence ao time.")
                .WithDescription("Adiciona um usuário ao time. O campo role aceita \"member\" ou \"administrator\" (sem diferenciar maiúsculas). Apenas o dono e os administradores do time podem adicionar usuários.");

            group
                .MapGet("teams/{teamId:guid}/members", async ([FromRoute] Guid teamId, IMediator mediator) =>
                {
                    var query = new GetTeamMembersQuery(teamId);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .ProducesWithDescription<TeamMembersDTO>(StatusCodes.Status200OK, "Os membros do time foram encontrados e retornados.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status403Forbidden, "O usuário não pertence ao time.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .WithDescription("Lista os membros de um time, ordenados por papel (dono, administradores e membros) e depois por nome. O campo role vem como \"owner\", \"administrator\" ou \"member\". Qualquer membro do time pode consultar.");

            group
                .MapPost("teams/{teamId:guid}/campaigns", async ([FromRoute] Guid teamId, [FromBody] CreateCampaignRequest request, IMediator mediator) =>
                {
                    var command = new CreateCampaignCommand(request.CampaignName, teamId);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Created($"teams/{teamId}/campaigns/{result.Value}", result.Value);
                })
                .ProducesWithDescription<Guid>(StatusCodes.Status201Created, "A campanha foi criada com sucesso.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .WithDescription("Cria uma campanha ativa no time informado.");

            group
                .MapGet("teams/{teamId:guid}/campaigns", async ([FromRoute] Guid teamId, IMediator mediator) =>
                {
                    var query = new GetTeamCampaignsQuery(teamId);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .ProducesWithDescription<TeamCampaignsDTO>(StatusCodes.Status200OK, "As campanhas do time foram encontradas e retornadas.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrado um time com o ID informado.")
                .WithDescription("Lista as campanhas de um time.");

            group
                .MapPost("teams/{teamId:guid}/campaigns/{campaignId:guid}/posts", async ([FromRoute]Guid teamId, [FromRoute]Guid campaignId, [FromBody]AssignPostToCampaignRequest request, IMediator mediator) =>
                {
                    var command = new AssignPostToCampaignCommand(campaignId, request.PostId);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok();
                })
                .ProducesWithDescription(StatusCodes.Status200OK, "A postagem foi associada à campanha.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "A postagem ou a campanha não foi encontrada.")
                .ProducesProblemWithDescription(StatusCodes.Status409Conflict, "A postagem já pertence à campanha.")
                .WithDescription("Associa uma postagem existente a uma campanha.");

            group
                .MapGet("teams/{teamId:guid}/campaigns/{campaignId:guid}/posts", async ([FromRoute] Guid teamId, [FromRoute] Guid campaignId, IMediator mediator) =>
                {
                    var query = new GetCampaignPostsQuery(teamId, campaignId);
                    var result = await mediator.Send(query);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok(result.Value);
                })
                .ProducesWithDescription<CampaignPostsDTO>(StatusCodes.Status200OK, "As postagens da campanha foram encontradas e retornadas.")
                .ProducesWithDescription(StatusCodes.Status401Unauthorized, "O usuário não está autenticado.")
                .ProducesProblemWithDescription(StatusCodes.Status404NotFound, "Não foi encontrada uma campanha com o ID informado.")
                .WithDescription("Lista as postagens de uma campanha.");

            return builder;
        }
    }
}
