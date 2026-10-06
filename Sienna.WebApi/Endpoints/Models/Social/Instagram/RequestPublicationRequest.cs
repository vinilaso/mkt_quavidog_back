using Sienna.Domain.Entities.Social;

namespace Sienna.WebApi.Endpoints.Models.Social.Instagram
{
    /// <summary>Dados para solicitar a publicação de uma postagem no Instagram do time.</summary>
    /// <param name="PostId">ID da postagem. Ela precisa estar em uma campanha do time e ter apenas imagens JPEG.</param>
    /// <param name="Format">Formato da publicação: "feed" (1 imagem ou carrossel de até 10) ou "story" (um story por imagem, até 10).</param>
    /// <param name="ScheduledFor">Horário da publicação, com fuso (ex.: 2026-10-10T18:00:00-03:00), no máximo 30 dias à frente. Vazio para publicar assim que possível.</param>
    public record RequestPublicationRequest(Guid PostId, PublicationFormat Format, DateTimeOffset? ScheduledFor);
}
