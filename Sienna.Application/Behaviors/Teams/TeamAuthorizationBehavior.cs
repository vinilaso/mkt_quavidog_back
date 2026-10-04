using MediatR;
using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.Behaviors.Teams
{
    public sealed class TeamAuthorizationBehavior<TRequest, TResponse>(
        IUserContext userContext,
        ITeamRepository teamRepository) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not ITeamScopedRequest teamRequest)
                return await next(cancellationToken);

            if (!userContext.IsAuthenticated)
            {
                var unauthorized = Error.Unauthorized("User.Authentication", "É necessário estar autenticado.");
                return ResultFailureFactory<TResponse>.Create(unauthorized);
            }

            var access = await teamRepository.GetAccessAsync(teamRequest.TeamId, userContext.Id, cancellationToken);

            if (access is null)
            {
                var notFound = Error.NotFound("Team.NotFound", "Não foi encontrado um time com o ID informado.");
                return ResultFailureFactory<TResponse>.Create(notFound);
            }

            if (access.Role is null)
            {
                var forbidden = Error.Forbidden("Team.Forbidden", "Você não possui acesso a este time.");
                return ResultFailureFactory<TResponse>.Create(forbidden);
            }

            if (request is ITeamManagerRequest && access.Role is not (TeamMemberRole.Owner or TeamMemberRole.Administrator))
            {
                var forbidden = Error.Forbidden("Team.NotManager", "Apenas o dono ou administradores do time podem realizar esta ação.");
                return ResultFailureFactory<TResponse>.Create(forbidden);
            }

            return await next(cancellationToken);
        }
    }
}
