using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Workflow.AssignPostToCampaign;
using Sienna.Application.UseCases.Workflow.CreateCampaign;
using Sienna.Application.UseCases.Workflow.CreateTeam;
using Sienna.Application.UseCases.Workflow.GetCampaignPosts;
using Sienna.Application.UseCases.Workflow.GetTeamCampaigns;
using Sienna.WebApi.Endpoints.Extensions;
using Sienna.WebApi.Endpoints.Models.Workflow.Campaigns;
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
                .WithDescription("Cria um time. O usuário logado é marcado como dono do time criado.");

            group
                .MapPost("teams/{teamId:guid}/campaigns", async ([FromRoute] Guid teamId, [FromBody] CreateCampaignRequest request, IMediator mediator) =>
                {
                    var command = new CreateCampaignCommand(request.CampaignName, teamId);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Created($"teams/{teamId}/campaigns/{result.Value}", result.Value);
                });

            group.MapGet("teams/{teamId:guid}/campaigns", async ([FromRoute] Guid teamId, IMediator mediator) =>
            {
                var query = new GetTeamCampaignsQuery(teamId);
                var result = await mediator.Send(query);

                if (result.IsFailure)
                    return result.Error.CreateProblemDetails();

                return TypedResults.Ok(result.Value);
            });

            group
                .MapPost("teams/{teamId:guid}/campaigns/{campaignId:guid}/posts", async ([FromRoute]Guid teamId, [FromRoute]Guid campaignId, [FromBody]AssignPostToCampaignRequest request, IMediator mediator) =>
                {
                    var command = new AssignPostToCampaignCommand(campaignId, request.PostId);
                    var result = await mediator.Send(command);

                    if (result.IsFailure)
                        return result.Error.CreateProblemDetails();

                    return TypedResults.Ok();
                });

            group.MapGet("teams/{teamId:guid}/campaigns/{campaignId:guid}/posts", async ([FromRoute] Guid teamId, [FromRoute] Guid campaignId, IMediator mediator) =>
            {
                var query = new GetCampaignPostsQuery(teamId, campaignId);
                var result = await mediator.Send(query);

                if (result.IsFailure)
                    return result.Error.CreateProblemDetails();

                return TypedResults.Ok(result.Value);
            });

            return builder;
        }
    }
}
