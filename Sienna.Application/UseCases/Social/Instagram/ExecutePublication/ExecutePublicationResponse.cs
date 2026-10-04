using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.ExecutePublication
{
    public record ExecutePublicationResponse(Guid PublicationId, PublicationStatus Status, string? FailureReason)
    {
        public static ExecutePublicationResponse FromPublication(PostPublication publication)
        {
            ArgumentNullException.ThrowIfNull(publication);
            return new(publication.Id, publication.Status, publication.FailureReason);
        }
    }
}
