using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.GetCampaignInsights
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="PublicationId"></param>
    /// <param name="PostId"></param>
    /// <param name="Caption"></param>
    /// <param name="Format"></param>
    /// <param name="PublishedAt"></param>
    /// <param name="LastCollectedAt"></param>
    /// <param name="Metrics"></param>
    public sealed record CampaignPublicationInsightsResponse(
        Guid PublicationId,
        Guid PostId,
        string Caption,
        PublicationFormat Format,
        DateTime PublishedAt,
        DateTime? LastCollectedAt,
        InsightTotalsDTO? Metrics
    );
}
