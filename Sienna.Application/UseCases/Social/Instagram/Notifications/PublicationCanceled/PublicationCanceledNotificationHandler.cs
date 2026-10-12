using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationCanceled
{
    internal sealed class PublicationCanceledNotificationHandler(
        IPostPublicationRepository publicationRepository,
        IEmailQueue emailQueue) : INotificationHandler<PublicationCanceledNotification>
    {
        public async Task Handle(PublicationCanceledNotification notification, CancellationToken cancellationToken)
        {
            if (await publicationRepository.GetNotificationDataAsync(notification.PublicationId, cancellationToken) is not PublicationNotificationDTO data)
                return;

            var message = new MailMessageBuilder()
                .AddRecipient(data.RequesterEmail, data.RequesterName)
                .AddSubject($"Sua publicação foi cancelada - {data.TeamName}")
                .AddPlainBody($"""
                    Olá, {data.RequesterName}

                    {notification.CanceledByName} cancelou a sua publicação. Ela não será enviada ao Instagram.

                    {PublicationEmailText.Summary(data)}
                    """)
                .Build();

            await emailQueue.EnqueueAsync(message, cancellationToken);
        }
    }
}
