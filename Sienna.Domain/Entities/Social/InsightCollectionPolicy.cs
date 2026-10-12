namespace Sienna.Domain.Entities.Social
{
    public static class InsightCollectionPolicy
    {
        public static readonly TimeSpan StoryWindow = TimeSpan.FromHours(24);
        public static readonly TimeSpan FeedWindow = TimeSpan.FromDays(30);
        public static readonly TimeSpan FeedIntensiveWindow = TimeSpan.FromHours(48);
        public static readonly TimeSpan IntensiveInterval = TimeSpan.FromHours(1);
        public static readonly TimeSpan RegularInterval = TimeSpan.FromHours(24);

        public static DateTime GetStoryStart(DateTime utcNow) => utcNow - StoryWindow;
        public static DateTime GetFeedStart(DateTime utcNow) => utcNow - FeedWindow;
        public static DateTime GetIntensiveStart(DateTime utcNow) => utcNow - FeedIntensiveWindow;
        public static DateTime GetIntensiveDue(DateTime utcNow) => utcNow - IntensiveInterval;
        public static DateTime GetRegularDue(DateTime utcNow) => utcNow - RegularInterval;
    }
}
