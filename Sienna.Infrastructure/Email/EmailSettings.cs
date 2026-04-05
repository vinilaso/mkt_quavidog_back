using System.ComponentModel.DataAnnotations;

namespace Sienna.Infrastructure.Email
{
    public sealed record EmailSettings
    {
        [Required(ErrorMessage = "A chave da API do fornecedor de e-mail não foi preenchida.")]
        public string ApiKey { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereço do remetente dos e-mails da aplicação não foi preenchido.")]
        [EmailAddress(ErrorMessage = "O endereço do remetente dos e-mails da aplicação não é um e-mail válido.")]
        public string SenderEmail { get; set; } = string.Empty;

        public string? SenderName { get; set; }
    }
}
