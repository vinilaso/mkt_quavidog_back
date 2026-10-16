using Sienna.Application.Extensions;
using Sienna.Domain.Abstractions.Social.DTOs;
using Sienna.Domain.Entities.Social;

namespace Sienna.Application.UseCases.Social.Instagram.GetCampaignInsights
{
    internal static class CampaignInsightsCalculator
    {
        internal static GetCampaignInsightsResponse Calculate(CampaignInsightsDataDTO data)
        {
            ArgumentNullException.ThrowIfNull(data);

            var publications = data.Publications
                .Select(publication =>
                {
                    var latest = publication.Snapshots.LastOrDefault();

                    return new CampaignPublicationInsightsResponse(
                        publication.PublicationId,
                        publication.PostId,
                        publication.Caption,
                        publication.Format,
                        publication.PublishedAt,
                        latest?.CollectedAt,
                        latest is null ? null : InsightTotalsDTO.From(latest)
                    );
                })
                .ToList();

            return new GetCampaignInsightsResponse(
                data.CampaignId,
                data.CampaignName,
                data.Publications.Count,
                CalculateTotals(publications),
                publications,
                CalculateDaily(data.Publications)
            );
        }

        private static CampaignInsightTotalsResponse CalculateTotals(IEnumerable<CampaignPublicationInsightsResponse> publications)
        {
            var metrics = publications
                .Where(publication => publication.Metrics is not null)
                .Select(publication => InsightMetrics.From(publication.Metrics!));

            var sum = InsightMetrics.Sum(metrics);

            return new CampaignInsightTotalsResponse(
                sum.Views,
                sum.Reach,
                sum.Likes,
                sum.Comments,
                sum.Shares,
                sum.Saved,
                sum.Replies,
                sum.TotalInteractions,
                InsightMetrics.CalculateEngagementRate(sum.TotalInteractions, sum.Reach)
            );
        }

        private static IReadOnlyList<DailyInsightResponse> CalculateDaily(IEnumerable<CampaignPublicationInsightDataDTO> publications)
        {
            var gainsByDate = new SortedDictionary<DateOnly, (int Views, int Reach, int Interactions)>();

            foreach (var publication in publications)
            {
                var lastOfEachDay = publication.Snapshots
                    .GroupBy(snapshot => DisplayTime.ToBrazilDate(snapshot.CollectedAt))
                    .Select(day => (Date: day.Key, Last: day.Last()))
                    .OrderBy(day => day.Date);

                InsightPointDTO? previous = null;

                foreach (var (date, last) in lastOfEachDay)
                {
                    var gain = (
                        Views: Gain(last.Views, previous?.Views),
                        Reach: Gain(last.Reach, previous?.Reach),
                        Interactions: Gain(last.TotalInteractions, previous?.TotalInteractions)
                    );

                    var current = gainsByDate.GetValueOrDefault(date);

                    gainsByDate[date] = (
                        Views: current.Views + gain.Views,
                        Reach: current.Reach + gain.Reach,
                        Interactions: current.Interactions + gain.Interactions
                    );

                    previous = last;
                }
            }

            return gainsByDate
                .Select(entry => new DailyInsightResponse(entry.Key, entry.Value.Views, entry.Value.Reach, entry.Value.Interactions))
                .ToList();
        }

        private static int Gain(int? current, int? previous) 
            => Math.Max(0, (current ?? previous ?? 0) - (previous ?? 0));
    }
}
