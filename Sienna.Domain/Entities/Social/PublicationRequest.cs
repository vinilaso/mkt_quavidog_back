namespace Sienna.Domain.Entities.Social
{
    public sealed record PublicationRequest
    {
        public required Guid PostId { get; init; }
        public required Guid TeamId { get; init; }
        public required PublicationFormat Format { get; init; }
        public required int MediaCount { get; init; }
        public DateTime? ScheduledFor { get; init; }
        public required Guid RequestedById { get; init; }
        public required bool RequesterIsManager { get; init; }
    }
}
