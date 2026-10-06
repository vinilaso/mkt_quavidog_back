using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram
{
    /// <summary>Publicação de uma postagem no Instagram de um time. Todas as datas estão em UTC.</summary>
    /// <param name="Id">ID da publicação.</param>
    /// <param name="PostId">ID da postagem publicada.</param>
    /// <param name="Format">Formato: "feed" ou "story".</param>
    /// <param name="Status">Situação atual da publicação.</param>
    /// <param name="ScheduledFor">Quando a publicação deve sair. Para "publicar agora", é o momento da solicitação.</param>
    /// <param name="RequestedById">ID do usuário que solicitou.</param>
    /// <param name="RequestedAt">Quando foi solicitada.</param>
    /// <param name="ReviewedById">ID do gestor que aprovou ou reprovou. Em solicitações de gestores, é o próprio solicitante.</param>
    /// <param name="ReviewedAt">Quando foi aprovada ou reprovada.</param>
    /// <param name="RejectionReason">Motivo da reprovação, quando reprovada.</param>
    /// <param name="PublishedAt">Quando foi publicada no Instagram.</param>
    /// <param name="ExternalIds">IDs gerados pelo Instagram: um para feed/carrossel, um por story.</param>
    /// <param name="FailureReason">Motivo da falha, quando o status é "failed".</param>
    public record PublicationResponse(
            Guid Id,
            Guid PostId,
            PublicationFormat Format,
            PublicationStatus Status,
            DateTime ScheduledFor,
            Guid RequestedById,
            DateTime RequestedAt,
            Guid? ReviewedById,
            DateTime? ReviewedAt,
            string? RejectionReason,
            DateTime? PublishedAt,
            IReadOnlyList<string> ExternalIds,
            string? FailureReason)
    {
        internal static PublicationResponse From(PostPublication publication)
        {
            ArgumentNullException.ThrowIfNull(publication);

            return new PublicationResponse(
                Id: publication.Id,
                PostId: publication.PostId,
                Format: publication.Format,
                Status: publication.Status,
                ScheduledFor: publication.ScheduledFor,
                RequestedById: publication.RequestedById,
                RequestedAt: publication.RequestedAt,
                ReviewedById: publication.ReviewedById,
                ReviewedAt: publication.ReviewedAt,
                RejectionReason: publication.RejectionReason,
                PublishedAt: publication.PublishedAt,
                ExternalIds: publication.ExternalIds,
                FailureReason: publication.FailureReason
            );
        }
    }
}
