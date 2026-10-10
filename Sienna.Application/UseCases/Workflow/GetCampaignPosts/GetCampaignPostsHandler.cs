using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.DTOs.Campaigns;
using Sienna.Domain.Abstractions.Workflow.Repositories;

namespace Sienna.Application.UseCases.Workflow.GetCampaignPosts
{
    public class GetCampaignPostsHandler(ICampaignRepository campaignRepository) : IRequestHandler<GetCampaignPostsQuery, Result<CampaignPostsDTO>>
    {
        public async Task<Result<CampaignPostsDTO>> Handle(GetCampaignPostsQuery request, CancellationToken cancellationToken)
        {
            var result = await campaignRepository.GetCampaignPostsAsync(request.TeamId, request.CampaignId, cancellationToken);

            if (result is null)
                return Error.NotFound("Campaign.NotFound", $"Não existe campanha com o ID informado.");

            return result;
        }
    }
}
