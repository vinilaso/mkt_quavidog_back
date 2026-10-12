namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed partial class InstagramClient
    {
        private sealed record InsightsResponse(IReadOnlyList<InsightMetricResponse> Data);
    }
}
