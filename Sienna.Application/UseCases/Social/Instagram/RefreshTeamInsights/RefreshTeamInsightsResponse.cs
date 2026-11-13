namespace Sienna.Application.UseCases.Social.Instagram.RefreshTeamInsights
{
    public sealed record RefreshTeamInsightsResponse(
        int Total,
        int Collected,
        int Skipped,
        IReadOnlyList<InsightCollectionFailure> Failures
    );

    public sealed record InsightCollectionFailure(
        Guid PublicationId,
        string Code,
        string Message
    );
}
