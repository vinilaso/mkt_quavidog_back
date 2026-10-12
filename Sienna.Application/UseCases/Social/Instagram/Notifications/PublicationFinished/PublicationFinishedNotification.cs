using MediatR;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationFinished
{
    internal sealed record PublicationFinishedNotification(Guid PublicationId) : INotification;
}
