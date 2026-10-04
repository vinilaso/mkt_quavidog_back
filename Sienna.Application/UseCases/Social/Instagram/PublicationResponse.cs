using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram
{
    public record PublicationResponse(
            Guid Id,
            Guid PostId,
            PublicationFormat Format,
            PublicationStatus Status,
            DateTime ScheduledFor,
            Guid RequestedById,
            DateTime RequestedAt,
            Guid? ReviewedById,
            DateTime? ReviewedAt,
            string? RejectionReason,
            DateTime? PublishedAt,
            IReadOnlyList<string> ExternalIds,
            string? FailureReason)
    {
        internal static PublicationResponse From(PostPublication publication)
        {
            ArgumentNullException.ThrowIfNull(publication);

            return new PublicationResponse(
                Id: publication.Id,
                PostId: publication.PostId,
                Format: publication.Format,
                Status: publication.Status,
                ScheduledFor: publication.ScheduledFor,
                RequestedById: publication.RequestedById,
                RequestedAt: publication.RequestedAt,
                ReviewedById: publication.ReviewedById,
                ReviewedAt: publication.ReviewedAt,
                RejectionReason: publication.RejectionReason,
                PublishedAt: publication.PublishedAt,
                ExternalIds: publication.ExternalIds,
                FailureReason: publication.FailureReason
            );
        }
    }
}
