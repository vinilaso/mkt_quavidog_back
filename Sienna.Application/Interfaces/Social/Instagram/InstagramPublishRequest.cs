using Sienna.Domain.Entities.Social;

namespace Sienna.Application.Interfaces.Social.Instagram
{
    public sealed record InstagramPublishRequest
    {
        public required InstagramCredentials Credentials { get; init; }
        public required PublicationFormat Format { get; init; }
        public required IReadOnlyList<Guid> MediaIds { get; init; }
        public string Caption { get; init; } = string.Empty;
    }
}
