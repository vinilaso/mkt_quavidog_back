using Sienna.Application.Interfaces.Email;

namespace Sienna.Infrastructure.Email.MailerSend
{
    internal static class MailerSendPayloadMapper
    {
        internal static MailerSendPayload MapMailerSend(this MailMessage mailMessage, EmailSettings settings)
        {
            return new MailerSendPayload(
                From: new MailerSendAddress(settings.SenderEmail, settings.SenderName),
                To: mailMessage.MapRecipients(),
                Subject: mailMessage.Subject,
                PlainBody: !mailMessage.IsHTML ? mailMessage.Body : null,
                HTMLBody: mailMessage.IsHTML ? mailMessage.Body : null,
                Attachments: mailMessage.MapAttachments()
            );
        }

        private static MailerSendAddress[] MapRecipients(this MailMessage mailMessage)
        {
            return [.. mailMessage.To.Select(address => new MailerSendAddress(address.Email, address.Name))];
        }

        private static MailerSendAttachment[] MapAttachments(this MailMessage mailMessage)
        {
            if (mailMessage.Attachments is null || mailMessage.Attachments.Count < 1)
                return [];

            return [.. mailMessage.Attachments.Select(attachment => new MailerSendAttachment(
                Content: attachment.Content,
                FileName: attachment.FileName,
                Disposition: attachment.Disposition.ToString().ToLower(),
                IdHTML: attachment.IdHTML
            ))];
        }
    }
}
