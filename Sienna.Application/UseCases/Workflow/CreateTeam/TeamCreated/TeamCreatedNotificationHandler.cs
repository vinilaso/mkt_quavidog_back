using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;

namespace Sienna.Application.UseCases.Workflow.CreateTeam.TeamCreated
{
    internal class TeamCreatedNotificationHandler(IEmailQueue emailQueue) : INotificationHandler<TeamCreatedNotification>
    {
        public async Task Handle(TeamCreatedNotification notification, CancellationToken cancellationToken)
        {
            var mailMessage = new MailMessageBuilder()
                .AddRecipient(notification.OwnerMail)
                .AddSubject("Time criado")
                .AddPlainBody($"O time {notification.TeamName} foi criado com sucesso.")
                .Build();

            await emailQueue.EnqueueAsync(mailMessage, cancellationToken);
        }
    }
}
