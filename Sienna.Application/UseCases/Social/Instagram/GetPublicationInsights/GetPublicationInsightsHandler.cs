using MediatR;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Social.Instagram.GetPublicationInsights
{
    public sealed class GetPublicationInsightsHandler(IPublicationInsightSnapshotRepository snapshotRepository) : IRequestHandler<GetPublicationInsightsQuery, Result<PublicationInsightsDTO>>
    {
        public async Task<Result<PublicationInsightsDTO>> Handle(GetPublicationInsightsQuery request, CancellationToken cancellationToken)
        {
            if (await snapshotRepository.GetPublicationInsightsAsync(request.TeamId, request.PublicationId, cancellationToken) is not PublicationInsightsDTO insights)
                return PublicationErrors.NotFound;

            return insights;
        }
    }
}
