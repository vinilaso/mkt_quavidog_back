using MediatR;
using Sienna.Application.UseCases.Social.Instagram.CollectPublicationInsights;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Social.Instagram.RefreshPublicationInsights
{
    public sealed class RefreshPublicationInsightsHandler(
        IPostPublicationRepository publicationRepository,
        IPublicationInsightSnapshotRepository snapshotRepository,
        ISender sender) : IRequestHandler<RefreshPublicationInsightsCommand, Result<PublicationInsightsDTO>>
    {
        public static readonly TimeSpan MinimumInterval = TimeSpan.FromMinutes(1);

        public async Task<Result<PublicationInsightsDTO>> Handle(RefreshPublicationInsightsCommand request, CancellationToken cancellationToken)
        {
            if (await publicationRepository.FindTeamPublicationByIdAsync(request.TeamId, request.PublicationId, cancellationToken) is null)
                return PublicationErrors.NotFound;

            var lastCollectedAt = await snapshotRepository.GetLastCollectedAtAsync(request.PublicationId, cancellationToken);

            if (lastCollectedAt is null || DateTime.UtcNow - lastCollectedAt.Value >= MinimumInterval)
            {
                var collected = await sender.Send(new CollectPublicationInsightsCommand(request.PublicationId), cancellationToken);

                if (collected.IsFailure)
                    return collected.Error;
            }

            if (await snapshotRepository.GetPublicationInsightsAsync(request.TeamId, request.PublicationId, cancellationToken) is not PublicationInsightsDTO insights)
                return PublicationErrors.NotFound;

            return insights;
        }
    }
}
