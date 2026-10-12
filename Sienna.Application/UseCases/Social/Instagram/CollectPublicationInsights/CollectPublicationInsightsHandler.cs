using MediatR;
using Sienna.Application.Interfaces.Social.Instagram;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.CollectPublicationInsights
{
    public sealed class CollectPublicationInsightsHandler(
        IPostPublicationRepository publicationRepository,
        IPublicationInsightSnapshotRepository snapshotRepository,
        InstagramCredentialsResolver credentialsResolver,
        IInstagramClient instagramClient,
        IUnitOfWork uow) : IRequestHandler<CollectPublicationInsightsCommand, Result<CollectPublicationInsightsResponse>>
    {
        public async Task<Result<CollectPublicationInsightsResponse>> Handle(CollectPublicationInsightsCommand request, CancellationToken cancellationToken)
        {
            if (await publicationRepository.FindByIdAsync(request.PublicationId, cancellationToken) is not PostPublication publication)
                return PublicationErrors.NotFound;

            if (publication.Status is not PublicationStatus.Published || publication.ExternalIds.Length == 0)
                return Error.Conflict("Insights.NotPublished", "Só é possível coletar métricas de publicações já publicadas no Instagram.");

            var credentials = await credentialsResolver.ResolveAsync(publication.TeamId, cancellationToken);

            if (credentials.IsFailure)
                return credentials.Error;

            List<InsightMetrics> metrics = [];

            foreach (var externalId in publication.ExternalIds)
            {
                var collected = await instagramClient.GetMediaInsightsAsync(credentials.Value, externalId, publication.Format, cancellationToken);

                if (collected.IsFailure)
                    return collected.Error;

                metrics.Add(collected.Value);
            }

            var total = InsightMetrics.Sum(metrics);
            var snapshot = PublicationInsightSnapshot.Create(publication.Id, total, DateTime.UtcNow);

            if (snapshot.IsFailure)
                return snapshot.Error;

            await snapshotRepository.AddAsync(snapshot.Value, cancellationToken);
            await uow.CommitChangesAsync(cancellationToken);

            return new CollectPublicationInsightsResponse(publication.Id, snapshot.Value.CollectedAt, total);
        }
    }
}
