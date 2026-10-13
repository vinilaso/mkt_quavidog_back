using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.DTOs
{
    /// <summary>
    /// Métricas de uma publicação, com os números mais recentes e o histórico de coletas.
    /// </summary>
    public class PublicationInsightsDTO
    {
        /// <summary>
        /// ID da publicação.
        /// </summary>
        public Guid PublicationId { get; set; }

        /// <summary>
        /// Formato da publicação.
        /// </summary>
        public PublicationFormat Format { get; set; }

        /// <summary>
        /// Situação da publicação. Apenas publicações com status "published" possuem métricas.
        /// </summary>
        public PublicationStatus Status { get; set; }

        /// <summary>
        /// Quando a publicação foi enviada ao Instagram, em UTC.
        /// </summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>
        /// Quando foi feita a coleta de métricas mais recente, em UTC. Nulo se ainda não houve coleta.
        /// </summary>
        public DateTime? LastCollectedAt { get; set; }

        /// <summary>
        /// Métricas mais recentes. Nulo se ainda não houve coleta.
        /// </summary>
        public InsightTotalsDTO? Totals { get; set; }

        /// <summary>
        /// Todas as coletas, da mais antiga para a mais recente.
        /// </summary>
        public IReadOnlyList<InsightPointDTO> Timeline { get; set; } = [];
    }
}
