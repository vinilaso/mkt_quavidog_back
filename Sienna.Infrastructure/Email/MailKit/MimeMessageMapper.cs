using MimeKit;
using MimeKit.Text;
using Sienna.Application.Interfaces.Email;

namespace Sienna.Infrastructure.Email.MailKit
{
    internal static class MimeMessageMapper
    {
        internal static MimeMessage MapMimeMessage(this MailMessage message, MailKitSettings settings)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(settings.SenderName, settings.SenderEmail));
            mimeMessage.To.AddRange(message.To.Select(r => new MailboxAddress(r.Name, r.Email)));

            var textFormat = message.IsHTML ? TextFormat.Html : TextFormat.Plain;
            mimeMessage.Body = new TextPart(textFormat) { Text = message.Body ?? string.Empty };

            return mimeMessage;
        }
    }
}
