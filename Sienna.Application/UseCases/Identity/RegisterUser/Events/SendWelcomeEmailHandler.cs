using MediatR;
using Microsoft.Extensions.Logging;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;

namespace Sienna.Application.UseCases.Identity.RegisterUser.Events
{
    public sealed class SendWelcomeEmailHandler(
        IEmailService emailService, 
        ILogger<SendWelcomeEmailHandler> logger) : INotificationHandler<UserRegisteredNotification>
    {
        public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            var message = new MailMessageBuilder()
                .AddRecipient(notification.Email, notification.FullName)
                .AddSubject("Usuário cadastrado")
                .AddPlainBody("Seu e-mail foi registrado com sucesso.")
                .Build();

            var result = await emailService.SendMessageAsync(message, cancellationToken);

            if (result.IsFailure)
            {
                logger.LogError("Falha ao enviar o e-mail de boas vindas para {Email}. Erro: {Error}", notification.Email, result.Error);
            }
        }
    }
}
