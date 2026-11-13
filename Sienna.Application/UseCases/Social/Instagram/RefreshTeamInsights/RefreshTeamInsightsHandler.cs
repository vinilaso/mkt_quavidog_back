using MediatR;
using Sienna.Application.UseCases.Social.Instagram.CollectPublicationInsights;
using Sienna.Application.UseCases.Social.Instagram.RefreshPublicationInsights;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Social.Instagram.RefreshTeamInsights
{
    public sealed class RefreshTeamInsightsHandler(
        IPostPublicationRepository publicationRepository,
        IPublicationInsightSnapshotRepository snapshotRepository,
        InstagramCredentialsResolver credentialsResolver,
        ISender sender) : IRequestHandler<RefreshTeamInsightsCommand, Result<RefreshTeamInsightsResponse>>
    {
        public async Task<Result<RefreshTeamInsightsResponse>> Handle(RefreshTeamInsightsCommand request, CancellationToken cancellationToken)
        {
            var credentials = await credentialsResolver.ResolveAsync(request.TeamId, cancellationToken);

            if (credentials.IsFailure)
                return credentials.Error;

            var now = DateTime.UtcNow;
            var publicationIds = await publicationRepository.GetTeamPublicationsForInsightsAsync(request.TeamId, now, cancellationToken);

            var collected = 0;
            var skipped = 0;
            List<InsightCollectionFailure> failures = [];

            foreach (var publicationId in publicationIds)
            {
                var lastCollectedAt = await snapshotRepository.GetLastCollectedAtAsync(publicationId, cancellationToken);

                if (lastCollectedAt is not null && now - lastCollectedAt.Value < RefreshPublicationInsightsHandler.MinimumInterval)
                {
                    skipped++;
                    continue;
                }

                var result = await sender.Send(new CollectPublicationInsightsCommand(publicationId), cancellationToken);

                if (result.IsSuccess)
                    collected++;
                else
                    failures.Add(new InsightCollectionFailure(publicationId, result.Error.Code, result.Error.Message));
            }

            return new RefreshTeamInsightsResponse(publicationIds.Count, collected, skipped, failures);
        }
    }
}
