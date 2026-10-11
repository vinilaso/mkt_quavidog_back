using Sienna.Application.UseCases.Social.Instagram.ReschedulePublication;

namespace Sienna.WebApi.Endpoints.Models.Social.Instagram
{
    /// <summary>Dados para reagendar uma publicação.</summary>
    /// <param name="ScheduledFor">Novo horário, com fuso (ex.: 2026-10-10T18:00:00-03:00), no máximo 30 dias à frente.</param>
    public record ReschedulePublicationRequest(DateTimeOffset ScheduledFor)
    {
        public ReschedulePublicationCommand ToCommand(Guid teamId, Guid publicationId) => new(teamId, publicationId, ScheduledFor.UtcDateTime);
    }
}
