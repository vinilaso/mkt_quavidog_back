using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationFinished
{
    internal sealed class PublicationFinishedNotificationHandler(
        IPostPublicationRepository publicationRepository,
        IEmailQueue emailQueue) : INotificationHandler<PublicationFinishedNotification>
    {
        public async Task Handle(PublicationFinishedNotification notification, CancellationToken cancellationToken)
        {
            if (await publicationRepository.GetNotificationDataAsync(notification.PublicationId, cancellationToken) is not PublicationNotificationDTO data)
                return;

            var (subject, body) = data.Status switch
            {
                PublicationStatus.Published => (
                    "Sua publicação está no ar",
                    "A publicação foi enviada ao Instagram com sucesso."
                ),

                PublicationStatus.Failed => (
                    "Falha ao publicar no Instagram.",
                    $"""
                    Não foi possível enviar a sua publicação ao Instagram.
                    Motivo: {data.FailureReason}

                    Confira o perfil no Instagram antes de solicitar novamente: em alguns casos, parte da publicação pode ter sido enviada.
                    """
                ),

                _ => (null, null)
            };

            if (subject is null)
                return;

            var message = new MailMessageBuilder()
                .AddRecipient(data.RequesterEmail, data.RequesterName)
                .AddSubject($"{subject} - {data.TeamName}")
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
