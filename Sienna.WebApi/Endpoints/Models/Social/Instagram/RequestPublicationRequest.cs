using Sienna.Domain.Entities.Social;

namespace Sienna.WebApi.Endpoints.Models.Social.Instagram
{
    public record RequestPublicationRequest(Guid PostId, PublicationFormat Format, DateTimeOffset? ScheduledFor);
}
