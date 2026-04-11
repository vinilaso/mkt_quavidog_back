using System.ComponentModel.DataAnnotations;

namespace Sienna.Infrastructure.Email.MailKit
{
    public record MailKitSettings
    {
        [Required(ErrorMessage = "O host SMTP não foi definido.")]
        public string Host { get; set; } = string.Empty;

        [Required(ErrorMessage = "A porta SMTP não foi definida.")]
        public int? Port { get; set; }

        [Required(ErrorMessage = "O endereço do remetente dos e-mails da aplicação não foi definido.")]
        [EmailAddress(ErrorMessage = "O endereço do remetente dos e-mails da aplicação não é um e-mail válido.")]
        public string SenderEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha de aplicativo da conta de e-mail remetente da aplicação não foi definida.")]
        public string AppPassword { get; set; } = string.Empty;

        public string? SenderName { get; set; } = string.Empty;
    }
}
