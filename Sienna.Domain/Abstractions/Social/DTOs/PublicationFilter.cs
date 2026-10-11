using Sienna.Domain.Abstractions.Pagination;
using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.Abstractions.Social.DTOs
{
    /// <summary>
    /// Filtros da listagem de publicações. Campos nulos ou vazios não filtram.
    /// </summary>
    public sealed record PublicationFilter
    {
        public IReadOnlyCollection<PublicationStatus> Statuses { get; init; } = [];
        public PublicationFormat? Format { get; init; }

        /// <summary>
        /// Início do período, em UTC (comparado com ScheduledFor).
        /// </summary>
        public DateTime? From { get; init; }

        /// <summary>
        /// Fim do período, em UTC (comparado com ScheduledFor).
        /// </summary>
        public DateTime? To { get; init; }

        /// <summary>
        /// Ordem por ScheduledFor. Padrão: as mais recentes primeiro.
        /// </summary>
        public SortDirection Order { get; init; } = SortDirection.Descending;
    }
}
