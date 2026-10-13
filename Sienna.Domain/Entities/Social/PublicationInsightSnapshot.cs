using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Domain.Entities.Social
{
    public class PublicationInsightSnapshot : IDbEntity
    {
        public Guid Id { get; set; }

        public Guid PublicationId { get; private set; }
        public PostPublication? Publication { get; private set; }

        public DateTime CollectedAt { get; private set; }

        public int? Views { get; private set; }
        public int? Reach { get; private set; }
        public int? Likes { get; private set; }
        public int? Comments { get; private set; }
        public int? Shares { get; private set; }
        public int? Saved { get; private set; }
        public int? Replies { get; private set; }
        public int? TotalInteractions { get; private set; }

        public double? EngagementRate => InsightMetrics.CalculateEngagementRate(TotalInteractions, Reach);

        protected PublicationInsightSnapshot()
        {
        }

        public static Result<PublicationInsightSnapshot> Create(Guid publicationId, InsightMetrics metrics, DateTime utcNow)
        {
            ArgumentNullException.ThrowIfNull(metrics);

            if (utcNow.Kind != DateTimeKind.Utc)
                throw new ArgumentException("O horário da coleta deve estar em UTC.", nameof(utcNow));

            if (metrics.HasNegativeValues)
                return Error.Validation("Insights.NegativeValue", "As métricas recebidas da Meta contêm valores negativos.");

            return new PublicationInsightSnapshot
            {
                Id = Guid.NewGuid(),
                PublicationId = publicationId,
                CollectedAt = utcNow,
                Views = metrics.Views,
                Reach = metrics.Reach,
                Likes = metrics.Likes,
                Comments = metrics.Comments,
                Shares = metrics.Shares,
                Saved = metrics.Saved,
                Replies = metrics.Replies,
                TotalInteractions = metrics.TotalInteractions
            };
        }
    }
}
