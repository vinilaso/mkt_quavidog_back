using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;
using Sienna.Domain.Abstractions.Workflow.Repositories;

namespace Sienna.Application.UseCases.Workflow.GetTeamMembers
{
    public sealed class GetTeamMembersHandler(ITeamRepository teamRepository) : IRequestHandler<GetTeamMembersQuery, Result<TeamMembersDTO>>
    {
        public async Task<Result<TeamMembersDTO>> Handle(GetTeamMembersQuery request, CancellationToken cancellationToken)
        {
            if (await teamRepository.GetTeamMembersAsync(request.TeamId, cancellationToken) is not TeamMembersDTO members)
                return Error.NotFound("Team.NotFound", "Não foi encontrado um time com ID informado.");

            return members;
        }
    }
}
