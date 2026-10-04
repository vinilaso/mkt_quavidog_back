using System.ComponentModel.DataAnnotations;

namespace Sienna.Infrastructure.Security.SecretProtection
{
    public record SecretProviderSettings
    {
        [Required(ErrorMessage = "A chave de criptografia de segredos (SecretProviderSettings:Key) não está configurada.")]
        public string Key { get; set; } = string.Empty;
    }
}
