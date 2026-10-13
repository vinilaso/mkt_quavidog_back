using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.DTOs
{
    /// <summary>
    /// Métricas mais recentes de uma publicação.
    /// </summary>
    public class InsightTotalsDTO : InsightPointDTO
    {
        /// <summary>
        /// Interações ÷ alcance (ex.: 0.149 = 14,9%). Nulo quando ainda não há alcance.
        /// </summary>
        public double? EngagementRate { get; set; }

        public static InsightTotalsDTO From(InsightPointDTO point) => new()
        {
            CollectedAt = point.CollectedAt,
            Views = point.Views,
            Reach = point.Reach,
            Likes = point.Likes,
            Comments = point.Comments,
            Shares = point.Shares,
            Saved = point.Saved,
            Replies = point.Replies,
            TotalInteractions = point.TotalInteractions,
            EngagementRate = InsightMetrics.CalculateEngagementRate(point.TotalInteractions, point.Reach)
        };
    }
}
