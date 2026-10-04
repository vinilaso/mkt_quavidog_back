using System.ComponentModel.DataAnnotations;

namespace Sienna.WebApi.HostedServices.Publications
{
    public record PublicationWorkerSettings
    {
        [Range(5, 3600)]
        public int IntervalSeconds { get; set; } = 30;

        [Range(1, 100)]
        public int BatchSize { get; set; } = 10;
    }
}
