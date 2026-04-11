using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions;

namespace Sienna.Infrastructure.Email.MailKit
{
    public sealed partial class MailKitService(
        ILogger<MailKitService> logger,
        IOptions<SmtpSettings> smtpSettings) : IEmailService
    {
        public async Task<Result> SendMessageAsync(MailMessage message, CancellationToken cancellationToken = default)
        {
            using var mimeMessage = message.MapMimeMessage(smtpSettings.Value);
            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(smtpSettings.Value.Host, smtpSettings.Value.Port.GetValueOrDefault(), SecureSocketOptions.StartTls, cancellationToken);
                await client.AuthenticateAsync(smtpSettings.Value.SenderEmail, smtpSettings.Value.AppPassword, cancellationToken);

                var response = await client.SendAsync(mimeMessage, cancellationToken);

                LogMessageSent(response);

                return Result.Success();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Falha ao enviar email.");
                return Error.Failure("Email.ProviderError", e.Message);
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
            }
        }

        [LoggerMessage(Level = LogLevel.Information, Message = "E-mail enviado. Resposta: {Response}")]
        private partial void LogMessageSent(string response);
    }
}
