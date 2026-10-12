using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.Interfaces.Social.Instagram
{
    public interface IInstagramClient
    {
        Task<Result<InstagramProfile>> GetProfileAsync(string accessToken, CancellationToken cancellationToken = default);
        Task<Result<IReadOnlyList<string>>> PublishAsync(InstagramPublishRequest request, CancellationToken cancellationToken = default);
        Task<Result<InsightMetrics>> GetMediaInsightsAsync(InstagramCredentials credentials, string mediaId, PublicationFormat format, CancellationToken cancellationToken = default);
    }
}
