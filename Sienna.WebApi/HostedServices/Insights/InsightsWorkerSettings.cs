using System.ComponentModel.DataAnnotations;

namespace Sienna.WebApi.HostedServices.Insights
{
    public record InsightsWorkerSettings
    {
        [Range(1, 60)]
        public int IntervalMinutes { get; set; } = 5;

        [Range(1, 200)]
        public int BatchSize { get; set; } = 20;

        [Range(1, 1440)]
        public int FailureCooldownMinutes { get; set; } = 60;
    }
}
