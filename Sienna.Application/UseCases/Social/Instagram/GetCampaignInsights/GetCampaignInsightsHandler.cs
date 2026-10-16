using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Social.Instagram.GetCampaignInsights
{
    public sealed class GetCampaignInsightsHandler(IPublicationInsightSnapshotRepository snapshotRepository) : IRequestHandler<GetCampaignInsightsQuery, Result<GetCampaignInsightsResponse>>
    {
        public async Task<Result<GetCampaignInsightsResponse>> Handle(GetCampaignInsightsQuery request, CancellationToken cancellationToken)
        {
            if (await snapshotRepository.GetCampaignInsightDataAsync(request.TeamId, request.CampaignId, cancellationToken) is not CampaignInsightsDataDTO data)
                return Error.NotFound("Campaign.NotFound", "Não foi encontrada uma campanha do time com o ID informado.");

            return CampaignInsightsCalculator.Calculate(data);
        }
    }
}
