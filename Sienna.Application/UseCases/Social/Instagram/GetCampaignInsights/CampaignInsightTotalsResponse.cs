namespace Sienna.Application.UseCases.Social.Instagram.GetCampaignInsights
{
    /// <summary>
    /// Soma das métricas mais recentes das publicações da campanha. 
    /// Métricas que não se aplicam a nenhuma publicação vêm nulas.
    /// </summary>
    /// <param name="Views">Quantidade de visualizações das publicações.</param>
    /// <param name="Reach">Quantidade de alcance das publicações.</param>
    /// <param name="Likes">Quantidade de curtidas das publicações.</param>
    /// <param name="Comments">Quantidade de comentários das publicações.</param>
    /// <param name="Shares">Quantidade de compartilhamentos das publicações.</param>
    /// <param name="Saved">Quantidade de salvamentos das publicações.</param>
    /// <param name="Replies">Quantidade de respostas das publicações.</param>
    /// <param name="TotalInteractions">Soma da quantidade total de interações das publicações.</param>
    /// <param name="EngagementRate">Taxa de engajamento das publicações.</param>
    public sealed record CampaignInsightTotalsResponse(
        int? Views,
        int? Reach,
        int? Likes,
        int? Comments,
        int? Shares,
        int? Saved,
        int? Replies,
        int? TotalInteractions,
        double? EngagementRate
    );
}
