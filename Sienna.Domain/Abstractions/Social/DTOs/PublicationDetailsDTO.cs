using Sienna.Domain.Abstractions.Media.DTOs;
using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.DTOs
{
    /// <summary>
    /// Publicação com todos os dados, a postagem e envolvidos.
    /// </summary>
    public class PublicationDetailsDTO
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
        /// Imagens da postagem, na ordem de publicação.
        /// </summary>
        public IEnumerable<AssetDTO> Assets { get; set; } = [];

        /// <summary>
        /// Formato: "feed" ou "story"
        /// </summary>
        public PublicationFormat Format { get; set; }

        /// <summary>
        /// Situação atual da publicação.
        /// </summary>
        public PublicationStatus Status { get; set; }

        /// <summary>
        /// Quando a publicação deve sair.
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
        /// ID do gestor que aprovou ou reprovou a publicação.
        /// </summary>
        public Guid? ReviewedById { get; set; }

        /// <summary>
        /// Nome do gestor que aprovou ou reprovou a publicação.
        /// </summary>
        public string? ReviewedByName { get; set; }

        /// <summary>
        /// Quando a publicação foi aprovada ou reprovada.
        /// </summary>
        public DateTime? ReviewedAt { get; set; }

        /// <summary>
        /// Motivo da recusa da publicação.
        /// </summary>
        public string? RejectionReason { get; set; }

        /// <summary>
        /// Quando a publicação foi publicada no Instagram, em UTC.
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// IDs gerados pelo Instagram: um para feed/carrossel, um por story.
        /// </summary>
        public string[] ExternalIds { get; set; } = [];

        /// <summary>
        /// Motivo de falha, quando o status é "failed"
        /// </summary>
        public string? FailureReason { get; set; }
    }
}
