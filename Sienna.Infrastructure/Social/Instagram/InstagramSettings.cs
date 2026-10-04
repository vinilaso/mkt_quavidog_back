using System.ComponentModel.DataAnnotations;

namespace Sienna.Infrastructure.Social.Instagram
{
    public record InstagramSettings
    {
        [Required(ErrorMessage = "A URL base do Instagram (InstagramSettings:GraphBaseUrl) não está configurada.")]
        public string GraphBaseUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "A URL pública de mídias (InstagramSettings:PublicMediaBaseUrl) não está configurada.")]
        public string PublicMediaBaseUrl { get; set; } = string.Empty;
    }
}
