using MediatR;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationCanceled
{
    internal sealed record PublicationCanceledNotification(Guid PublicationId, string CanceledByName) : INotification;
}
