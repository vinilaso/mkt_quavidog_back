using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Application.Interfaces.Email.Templates.UseCases.RegisterUser;

namespace Sienna.Application.UseCases.Identity.RegisterUser.Events
{
    public sealed class SendWelcomeEmailHandler(IEmailQueue emailQueue) : INotificationHandler<UserRegisteredNotification>
    {
        public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            var message = new MailMessageBuilder()
                .AddRecipient(notification.Email, notification.FullName)
                .AddTemplate(new RegisterUserMailTemplate())
                .WithVariables(new RegisterUserTemplateVariables(notification.FullName))
                .Build();

            await emailQueue.EnqueueAsync(message, cancellationToken);
        }
    }
}
