using Sienna.Domain.Abstractions.Social.DTOs;

namespace Sienna.Tests.Common.Builders.Social
{
    public static class InsightPoints
    {
        /// <summary>Uma coleta de métricas. Só os valores informados são preenchidos.</summary>
        public static InsightPointDTO Point(
            DateTime collectedAt,
            int? views = null,
            int? reach = null,
            int? interactions = null,
            int? likes = null,
            int? comments = null,
            int? shares = null,
            int? saved = null,
            int? replies = null) => new()
        {
            CollectedAt = collectedAt,
            Views = views,
            Reach = reach,
            TotalInteractions = interactions,
            Likes = likes,
            Comments = comments,
            Shares = shares,
            Saved = saved,
            Replies = replies
        };
    }
}
