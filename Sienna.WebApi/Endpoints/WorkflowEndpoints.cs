using Sienna.Application.UseCases.Workflow.CreateTeam;
using Sienna.WebApi.Endpoints.Extensions;

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

            return builder;
        }
    }
}
