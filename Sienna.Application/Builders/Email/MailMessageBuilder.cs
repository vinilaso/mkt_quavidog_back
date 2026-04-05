using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Exceptions.Email;

namespace Sienna.Application.Builders.Email
{
    public class MailMessageBuilder
    {
        private readonly List<MailAddress> _recipients = [];
        private string? _subject;
        private string? _body;
        private bool _isHtml;

        public MailMessageBuilder AddRecipient(MailAddress recipient)
        {
            ArgumentNullException.ThrowIfNull(recipient, nameof(recipient));

            if (string.IsNullOrWhiteSpace(recipient.Email))
                throw new InvalidEmailException("message.To.*.Email", "Não é possível adicionar um destinatário sem um endereço de e-mail.");

            _recipients.Add(recipient);
            return this;
        }

        public MailMessageBuilder AddRecipient(string recipientEmail, string? recipientName = default)
        {
            return AddRecipient(new(recipientEmail, recipientName));
        }

        public MailMessageBuilder AddSubject(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new InvalidEmailException("message.Subject", "Não é possivel adicionar um assunto vazio a um e-mail.");

            _subject = subject;
            return this;
        }

        public MailMessageBuilder AddPlainBody(string plainBody)
        {
            if (string.IsNullOrWhiteSpace(plainBody))
                throw new InvalidEmailException("message.Body", "Não é possível adicionar um corpo vazio ao e-mail.");

            _body = plainBody;
            _isHtml = false;
            return this;
        }

        public MailMessageBuilder AddHTMLBody(string htmlBody)
        {
            if (string.IsNullOrWhiteSpace(htmlBody))
                throw new InvalidEmailException("message.Body", "Não é possível adicionar um corpo vazio ao e-mail.");

            _body = htmlBody;
            _isHtml = true;
            return this;
        }

        public MailMessage Build()
        {
            return new MailMessage
            {
                To = _recipients,
                Body = _body,
                IsHTML = _isHtml,
                Subject = _subject ?? string.Empty
            };
        }
    }
}
