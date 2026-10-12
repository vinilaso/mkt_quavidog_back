using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.CollectPublicationInsights
{
    /// <param name="PublicationId">ID da publicação.</param>
    /// <param name="CollectedAt">Quando as métricas foram coletadas, em UTC.</param>
    /// <param name="Metrics">As métricas coletadas (somadas, no caso de stories).</param>
    public sealed record CollectPublicationInsightsResponse(
        Guid PublicationId,
        DateTime CollectedAt,
        InsightMetrics Metrics
    );
}
