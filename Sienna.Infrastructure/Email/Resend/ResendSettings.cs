using System.ComponentModel.DataAnnotations;

namespace Sienna.Infrastructure.Email.Resend
{
    public record ResendSettings
    {
        [Required(ErrorMessage = "A chave de API do serviço de e-mail não está configurada.")]
        public string ApiKey { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "O endereço do remetente dos e-mails da aplicação não está configurado.")]
        public string SenderAddress { get; set; } = string.Empty;

        public string? SenderName { get; set; } = string.Empty;
    }
}
