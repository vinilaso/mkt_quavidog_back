using MediatR;
using Microsoft.Extensions.Logging;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;

namespace Sienna.Application.UseCases.Identity.Login.Events
{
    public sealed class SendLockedOutEmailHandler(
        IEmailService emailService,
        ILogger<SendLockedOutEmailHandler> logger) : INotificationHandler<UserLockedOutNotification>
    {
        public async Task Handle(UserLockedOutNotification notification, CancellationToken cancellationToken)
        {
            var mailMessage = new MailMessageBuilder()
                .AddRecipient(notification.Email, notification.FullName)
                .AddSubject("Sua conta foi bloqueada.")
                .AddPlainBody("O limite de tentativas de login foi excedido. Sua conta está bloqueada.")
                .Build();

            var result = await emailService.SendMessageAsync(mailMessage, cancellationToken);

            if (result.IsFailure)
            {
                logger.LogError("Erro ao enviar o e-mail de conta bloqueada a {Email}. Erro: {Error}", notification.Email, result.Error.Message);
            }
        }
    }
}
