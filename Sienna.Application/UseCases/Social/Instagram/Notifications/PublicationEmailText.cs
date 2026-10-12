using Sienna.Application.Extensions;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.Notifications
{
    internal static class PublicationEmailText
    {
        private const int CaptionPreviewLength = 120;

        internal static string Summary(PublicationNotificationDTO data)
        {
            var format = data.Format is PublicationFormat.Story ? "stories" : "feed";
            var caption = data.Caption.Length > CaptionPreviewLength
                ? data.Caption[..CaptionPreviewLength] + "..."
                : data.Caption;

            return $"""
                Time: {data.TeamName}
                Formato: {format}
                Horário agendado: {DisplayTime.Format(data.ScheduledFor)} (horário de Brasília)
                Legenda: {(string.IsNullOrWhiteSpace(caption) ? "(sem legenda)" : caption)}
                """;
        }
    }
}
