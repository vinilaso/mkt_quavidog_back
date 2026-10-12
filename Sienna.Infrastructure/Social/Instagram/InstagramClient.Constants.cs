using System.Text.Json;

namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed partial class InstagramClient
    {
        private const string FeedInsightMetrics = "views,reach,likes,comments,shares,saved,total_interactions";
        private const string StoryInsightMetrics = "views,reach,replies,shares,total_interactions";
        private const int MaxStatusChecks = 15;
        private static readonly TimeSpan StatusCheckInterval = TimeSpan.FromSeconds(2);
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
