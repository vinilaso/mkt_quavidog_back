using MediatR;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationReviewed
{
    internal sealed record PublicationReviewedNotification(Guid PublicationId) : INotification;
}
