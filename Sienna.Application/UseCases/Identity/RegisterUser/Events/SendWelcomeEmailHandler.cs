using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;

namespace Sienna.Application.UseCases.Identity.RegisterUser.Events
{
    public sealed class SendWelcomeEmailHandler(IEmailQueue emailQueue) : INotificationHandler<UserRegisteredNotification>
    {
        public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            var message = new MailMessageBuilder()
                .AddRecipient(notification.Email, notification.FullName)
                .AddSubject("Usuário cadastrado")
                .AddPlainBody("Seu e-mail foi registrado com sucesso.")
                .Build();

            await emailQueue.EnqueueAsync(message, cancellationToken);
        }
    }
}
