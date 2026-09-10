using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Teams;
using Sienna.Domain.Abstractions.Workflow.Repositories;

namespace Sienna.Application.UseCases.Workflow.GetTeamCampaigns
{
    public class GetTeamCampaignsHandler(ITeamRepository teamRepository) : IRequestHandler<GetTeamCampaignsQuery, Result<TeamCampaignsDTO>>
    {
        public async Task<Result<TeamCampaignsDTO>> Handle(GetTeamCampaignsQuery request, CancellationToken cancellationToken)
        {
            var result = await teamRepository.GetTeamCampaignsAsync(request.TeamId, cancellationToken);

            if (result is null)
                return Error.NotFound("Team.NotFound", $"Não foi encontrado um time com o ID informado.");

            return result;
        }
    }
}
