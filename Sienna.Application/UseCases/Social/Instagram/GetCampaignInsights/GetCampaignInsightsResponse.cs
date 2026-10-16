using Sienna.Domain.Abstractions.Social.DTOs;

namespace Sienna.Application.UseCases.Social.Instagram.GetCampaignInsights
{
    /// <summary>
    /// Métricas da campanha no Instagram.
    /// </summary>
    /// <param name="CampaignId">ID da campanha.</param>
    /// <param name="CampaignName">Nome da campanha.</param>
    /// <param name="PublicationCount">Quantidade de publicações já publicadas da campanha.</param>
    /// <param name="Totals">Soma das métricas mais recentes de todas as publicações da campanha.</param>
    /// <param name="Publications">Métricas mais recentes de cada publicação, na ordem de publicação.</param>
    /// <param name="Daily">Ganhos de cada dia (horário de Brasília), somando todas as publicações.</param>
    public sealed record GetCampaignInsightsResponse(
        Guid CampaignId,
        string CampaignName,
        int PublicationCount,
        CampaignInsightTotalsResponse Totals,
        IReadOnlyList<CampaignPublicationInsightsResponse> Publications,
        IReadOnlyList<DailyInsightResponse> Daily
    );
}
