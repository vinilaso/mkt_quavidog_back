namespace Sienna.Application.UseCases.Social.Instagram.GetCampaignInsights
{
    /// <summary>
    /// O que as publicações de uma campanha ganharam em um dia.
    /// </summary>
    /// <param name="Date">Dia, no horário de Brasília.</param>
    /// <param name="Views">Visualizações ganhas no dia (somado entre as publicações).</param>
    /// <param name="Reach">Alcance ganho no dia (somado entre as publicações).</param>
    /// <param name="TotalInteractions">Interações ganhas no dia (somado entre as publicações).</param>
    public sealed record DailyInsightResponse(
        DateOnly Date,
        int Views,
        int Reach,
        int TotalInteractions
    );
}
