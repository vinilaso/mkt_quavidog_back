using Sienna.Shared.Extensions.System.Collections.Genenric;

namespace Sienna.Domain.Entities.Social
{
    /// <summary>
    /// Métricas de uma publicação no Instagram em um momento. Valores nulos indicam métricas
    /// que não existem para o formato (ex.: "saved" em stories, "replies" no feed) ou que a Meta não devolveu.
    /// </summary>
    public sealed record InsightMetrics
    {
        public int? Views { get; init; }
        public int? Reach { get; init; }
        public int? Likes { get; init; }
        public int? Comments { get; init; }
        public int? Shares { get; init; }
        public int? Saved { get; init; }
        public int? Replies { get; init; }
        public int? TotalInteractions { get; init; }

        internal bool HasNegativeValues 
            => new[] { Views, Reach, Likes, Comments, Shares, Saved, Replies, TotalInteractions }.Any(value => value < 0);

        public static InsightMetrics Sum(IEnumerable<InsightMetrics> metrics)
        {
            var metricList = metrics.ToList();

            return new InsightMetrics
            {
                Views = metricList.SumOf(metric => metric.Views),
                Reach = metricList.SumOf(metric => metric.Reach),
                Likes = metricList.SumOf(metric => metric.Likes),
                Comments = metricList.SumOf(metric => metric.Comments),
                Shares = metricList.SumOf(metric => metric.Shares),
                Saved = metricList.SumOf(metric => metric.Saved),
                Replies = metricList.SumOf(metric => metric.Replies),
                TotalInteractions = metricList.SumOf(metric => metric.TotalInteractions)
            };
        }
    }
}
