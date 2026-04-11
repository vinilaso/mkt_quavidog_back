using Sienna.Application.Interfaces.Email;

namespace Sienna.Infrastructure.Email.Resend
{
    internal static class ResendPayloadMapper
    {
        internal static ResendSendPayload MapSend(this MailMessage message, ResendSettings settings)
        {
            return new ResendSendPayload(
                From: settings.GetFromParameter(),
                To: [.. from r in message.To select r.Email],
                Subject: message.Subject,
                Html: message.IsHTML ? message.Body : null,
                Text: !message.IsHTML ? message.Body : null
            );
        }

        private static string GetFromParameter(this ResendSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.SenderName))
                return settings.SenderAddress;

            return $"{settings.SenderName} <{settings.SenderAddress}>";
        }
    }
}
