using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationReviewed
{
    internal sealed class PublicationReviewedNotificationHandler(
        IPostPublicationRepository publicationRepository,
        IEmailQueue emailQueue) : INotificationHandler<PublicationReviewedNotification>
    {
        public async Task Handle(PublicationReviewedNotification notification, CancellationToken cancellationToken)
        {
            if (await publicationRepository.GetNotificationDataAsync(notification.PublicationId, cancellationToken) is not PublicationNotificationDTO data)
                return;

            var (subject, body) = data.Status switch
            {
                PublicationStatus.Approved => (
                    "Sua publicação foi aprovada",
                    $"{data.ReviewerName} aprovou a sua publicação. Ela será enviada para o Instagram no horário agendado."
                ),

                PublicationStatus.Rejected => (
                    "Sua publicação foi reprovada",
                    $"{data.ReviewerName} reprovou a sua publicação.\nMotivo: {data.RejectionReason}"
                ),

                _ => (null, null)
            };

            if (subject is null)
                return;

            var message = new MailMessageBuilder()
                .AddRecipient(data.RequesterEmail, data.RequesterName)
                .AddSubject(subject)
                .AddPlainBody($"""
                    Olá, {data.RequesterName}.

                    {body}

                    {PublicationEmailText.Summary(data)}
                    """)
                .Build();

            await emailQueue.EnqueueAsync(message, cancellationToken);
        }
    }
}
