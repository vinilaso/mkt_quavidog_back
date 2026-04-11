using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;

namespace Sienna.Application.UseCases.Identity.Login.Events
{
    public sealed class SendLockedOutEmailHandler(IEmailQueue emailQueue) : INotificationHandler<UserLockedOutNotification>
    {
        public async Task Handle(UserLockedOutNotification notification, CancellationToken cancellationToken)
        {
            var mailMessage = new MailMessageBuilder()
                .AddRecipient(notification.Email, notification.FullName)
                .AddSubject("Sua conta foi bloqueada.")
                .AddPlainBody("O limite de tentativas de login foi excedido. Sua conta está bloqueada.")
                .Build();

            await emailQueue.EnqueueAsync(mailMessage, cancellationToken);
        }
    }
}
