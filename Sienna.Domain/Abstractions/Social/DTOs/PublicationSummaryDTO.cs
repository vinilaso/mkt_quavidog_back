using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.DTOs
{
    /// <summary>
    /// Publicação resumida, para listagens.
    /// </summary>
    public class PublicationSummaryDTO
    {
        /// <summary>
        /// ID da publicação.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID da postagem.
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// Legenda da postagem.
        /// </summary>
        public string Caption { get; set; } = string.Empty;

        /// <summary>
        /// ID da primeira imagem da postagem, para exibir uma miniatura.
        /// </summary>
        public Guid? ThumbnailMediaId { get; set; }

        /// <summary>
        /// Quantidade de imagens da postagem.
        /// </summary>
        public int MediaCount { get; set; }

        /// <summary>
        /// Formato: "feed" ou "story"
        /// </summary>
        public PublicationFormat Format { get; set; }

        /// <summary>
        /// Situação atual da publicação.
        /// </summary>
        public PublicationStatus Status { get; set; }

        /// <summary>
        /// Quando a publicação deve sair, em UTC.
        /// </summary>
        public DateTime ScheduledFor { get; set; }

        /// <summary>
        /// ID de quem solicitou a publicação.
        /// </summary>
        public Guid RequestedById { get; set; }

        /// <summary>
        /// Nome de quem solicitou a publicação.
        /// </summary>
        public string RequestedByName { get; set; } = string.Empty;

        /// <summary>
        /// Quando a publicação foi solicitada, em UTC.
        /// </summary>
        public DateTime RequestedAt { get; set; }

        /// <summary>
        /// Quando a publicação foi publicada no Instagram, em UTC.
        /// </summary>
        public DateTime? PublishedAt { get; set; }
    }
}
