using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions.Social.Repositories;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications.PublicationRequested
{
    internal sealed class PublicationRequestedNotificationHandler(
        IPostPublicationRepository publicationRepository,
        ITeamRepository teamRepository,
        IEmailQueue emailQueue) : INotificationHandler<PublicationRequestedNotification>
    {
        public async Task Handle(PublicationRequestedNotification notification, CancellationToken cancellationToken)
        {
            if (await publicationRepository.GetNotificationDataAsync(notification.PublicationId, cancellationToken) is not { Status: PublicationStatus.PendingApproval } data)
                return;

            var managers = await teamRepository.GetManagerContactsAsync(data.TeamId, cancellationToken);

            foreach (var manager in managers)
            {
                var message = new MailMessageBuilder()
                    .AddRecipient(manager.Email, manager.Name)
                    .AddSubject($"Nova publicação aguardando aprovação - {data.TeamName}")
                    .AddPlainBody($"""
                         Olá, {manager.Name}.

                         {data.RequesterName} solicitou uma publicação no Instagram do time e ela aguarda sua aprovação.

                         {PublicationEmailText.Summary(data)}

                         Acesse o sistema para aprovar ou reprovar.
                         """)
                    .Build();

                await emailQueue.EnqueueAsync(message, cancellationToken);
            }
        }
    }
}
