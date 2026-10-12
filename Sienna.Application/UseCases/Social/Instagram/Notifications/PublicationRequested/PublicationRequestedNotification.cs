using MediatR;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationRequested
{
    internal sealed record PublicationRequestedNotification(Guid PublicationId) : INotification;
}
