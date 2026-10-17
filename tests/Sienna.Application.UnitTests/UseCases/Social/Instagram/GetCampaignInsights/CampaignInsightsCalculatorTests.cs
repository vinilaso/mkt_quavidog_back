using Sienna.Application.UseCases.Social.Instagram.GetCampaignInsights;
using Sienna.Domain.Entities.Social;
using Sienna.Tests.Common.Builders.Social;
using static Sienna.Tests.Common.Builders.Social.InsightPoints;
using static Sienna.Tests.Common.Time.TestTime;

namespace Sienna.Application.UnitTests.UseCases.Social.Instagram.GetCampaignInsights
{
    [TestClass]
    public sealed class CampaignInsightsCalculatorTests
    {
        private static readonly DateOnly Oct12 = new(2026, 10, 12);
        private static readonly DateOnly Oct13 = new(2026, 10, 13);
        private static readonly DateOnly Oct14 = new(2026, 10, 14);
        private static readonly DateOnly Oct15 = new(2026, 10, 15);

        private static CampaignInsightDataBuilder Campaign() => new();

        #region Cabeçalho, publicações e totais

        [TestMethod]
        public void Calculate_CopiesCampaignHeader()
        {
            var data = Campaign().Named("Promoção de outubro").WithPublication().WithPublication().Build();

            var response = CampaignInsightsCalculator.Calculate(data);

            Assert.AreEqual(data.CampaignId, response.CampaignId);
            Assert.AreEqual("Promoção de outubro", response.CampaignName);
            Assert.AreEqual(2, response.PublicationCount);
        }

        [TestMethod]
        public void Calculate_WithoutPublications_ReturnsEmptyListsAndNullTotals()
        {
            var response = CampaignInsightsCalculator.Calculate(Campaign().Build());

            Assert.AreEqual(0, response.PublicationCount);
            Assert.IsEmpty(response.Publications);
            Assert.IsEmpty(response.Daily);
            Assert.AreEqual(new CampaignInsightTotalsResponse(null, null, null, null, null, null, null, null, null), response.Totals);
        }

        [TestMethod]
        public void Calculate_UsesLatestSnapshotOfEachPublication()
        {
            var latest = Brasilia(2026, 10, 13, 20);
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 12, 20), views: 100, reach: 80, interactions: 8),
                    Point(latest, views: 300, reach: 200, interactions: 30))
                .Build();

            var publication = Assert.ContainsSingle(CampaignInsightsCalculator.Calculate(data).Publications);

            Assert.AreEqual(latest, publication.LastCollectedAt);
            Assert.IsNotNull(publication.Metrics);
            Assert.AreEqual(300, publication.Metrics.Views);
            Assert.AreEqual(0.15, publication.Metrics.EngagementRate);
        }

        [TestMethod]
        public void Calculate_PublicationWithoutSnapshots_HasNullMetricsAndIsNotSummed()
        {
            var data = Campaign()
                .WithPublication(Point(Brasilia(2026, 10, 12, 20), views: 100))
                .WithPublication()
                .Build();

            var response = CampaignInsightsCalculator.Calculate(data);

            Assert.AreEqual(2, response.PublicationCount);
            Assert.IsNull(response.Publications[1].Metrics);
            Assert.IsNull(response.Publications[1].LastCollectedAt);
            Assert.AreEqual(100, response.Totals.Views);
        }

        [TestMethod]
        public void Calculate_KeepsPublicationOrder()
        {
            var data = Campaign().WithPublication().WithPublication().WithPublication().Build();

            var response = CampaignInsightsCalculator.Calculate(data);

            Assert.AreSequenceEqual(
                data.Publications.Select(publication => publication.PublicationId),
                response.Publications.Select(publication => publication.PublicationId));
        }

        [TestMethod]
        public void Calculate_SumsLatestMetricsOfAllPublications()
        {
            var day = Brasilia(2026, 10, 12, 20);
            var data = Campaign()
                .WithPublication(Point(day, views: 1000, reach: 800, interactions: 90, likes: 70, comments: 10))
                .WithPublication(Point(day, views: 500, reach: 300, interactions: 30, likes: 20, comments: 5))
                .Build();

            var totals = CampaignInsightsCalculator.Calculate(data).Totals;

            Assert.AreEqual(1500, totals.Views);
            Assert.AreEqual(1100, totals.Reach);
            Assert.AreEqual(120, totals.TotalInteractions);
            Assert.AreEqual(90, totals.Likes);
            Assert.AreEqual(15, totals.Comments);
        }

        [TestMethod]
        public void Calculate_WithFeedAndStory_KeepsFormatSpecificMetrics()
        {
            var day = Brasilia(2026, 10, 12, 20);
            var data = Campaign()
                .WithPublication(PublicationFormat.Feed, Point(day, views: 100, saved: 7))
                .WithPublication(PublicationFormat.Story, Point(day, views: 50, replies: 3))
                .Build();

            var totals = CampaignInsightsCalculator.Calculate(data).Totals;

            Assert.AreEqual(7, totals.Saved);
            Assert.AreEqual(3, totals.Replies);
            Assert.IsNull(totals.Shares);
        }

        [TestMethod]
        public void Calculate_TotalEngagementRate_IsWeightedByReach()
        {
            // A: 50 / 100 = 50%   B: 10 / 900 ≈ 1,1%
            // Média simples das taxas: 25,6%. Correto: 60 / 1000 = 6%
            var day = Brasilia(2026, 10, 12, 20);
            var data = Campaign()
                .WithPublication(Point(day, reach: 100, interactions: 50))
                .WithPublication(Point(day, reach: 900, interactions: 10))
                .Build();

            Assert.AreEqual(0.06, CampaignInsightsCalculator.Calculate(data).Totals.EngagementRate);
        }

        [TestMethod]
        public void Calculate_WithZeroReach_ReturnsNullEngagementRate()
        {
            var data = Campaign().WithPublication(Point(Brasilia(2026, 10, 12, 20), reach: 0, interactions: 0)).Build();

            Assert.IsNull(CampaignInsightsCalculator.Calculate(data).Totals.EngagementRate);
        }

        [TestMethod]
        public void Calculate_WithUnorderedSnapshots_UsesChronologicalOrder()
        {
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 13, 23), views: 300),
                    Point(Brasilia(2026, 10, 13, 10), views: 200),
                    Point(Brasilia(2026, 10, 12, 23), views: 100))
                .Build();

            var response = CampaignInsightsCalculator.Calculate(data);

            Assert.AreEqual(300, response.Publications[0].Metrics?.Views);
            Assert.AreSequenceEqual(new[] { Oct12, Oct13 }, response.Daily.Select(day => day.Date));
            Assert.AreSequenceEqual(new[] { 100, 200 }, response.Daily.Select(day => day.Views));
        }

        #endregion

        #region Ganhos diários

        [TestMethod]
        public void Daily_IsLastValueOfTheDayMinusLastValueOfThePreviousDay()
        {
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 12, 10), views: 100, reach: 80, interactions: 5),
                    Point(Brasilia(2026, 10, 12, 23), views: 400, reach: 300, interactions: 20),
                    Point(Brasilia(2026, 10, 13, 12), views: 700, reach: 500, interactions: 40),
                    Point(Brasilia(2026, 10, 13, 23), views: 900, reach: 600, interactions: 60),
                    Point(Brasilia(2026, 10, 14, 23), views: 1000, reach: 650, interactions: 65))
                .Build();

            var daily = CampaignInsightsCalculator.Calculate(data).Daily;

            Assert.AreSequenceEqual(new DailyInsightResponse[]
            {
                new(Oct12, Views: 400, Reach: 300, TotalInteractions: 20),
                new(Oct13, Views: 500, Reach: 300, TotalInteractions: 40),
                new(Oct14, Views: 100, Reach: 50, TotalInteractions: 5)
            }, daily);
        }

        [TestMethod]
        public void Daily_SumsGainsOfAllPublicationsOnTheSameDate()
        {
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 12, 22), views: 100, reach: 80, interactions: 10),
                    Point(Brasilia(2026, 10, 13, 22), views: 150, reach: 100, interactions: 12))
                .WithPublication(
                    Point(Brasilia(2026, 10, 13, 22), views: 200, reach: 150, interactions: 30))
                .Build();

            var daily = CampaignInsightsCalculator.Calculate(data).Daily;

            Assert.AreSequenceEqual(new DailyInsightResponse[]
            {
                new(Oct12, Views: 100, Reach: 80, TotalInteractions: 10),
                new(Oct13, Views: 250, Reach: 170, TotalInteractions: 32)
            }, daily);
        }

        [TestMethod]
        public void Daily_GroupsByBrasiliaDate()
        {
            var data = Campaign()
                .WithPublication(
                    Point(new DateTime(2026, 10, 13, 1, 30, 0, DateTimeKind.Utc), views: 10, reach: 5, interactions: 1), // 12/10 22:30 em Brasília
                    Point(new DateTime(2026, 10, 13, 3, 0, 0, DateTimeKind.Utc), views: 30, reach: 9, interactions: 4))  // 13/10 00:00 em Brasília
                .Build();

            var daily = CampaignInsightsCalculator.Calculate(data).Daily;

            Assert.AreSequenceEqual(new DailyInsightResponse[]
            {
                new(Oct12, Views: 10, Reach: 5, TotalInteractions: 1),
                new(Oct13, Views: 20, Reach: 4, TotalInteractions: 3)
            }, daily);
        }

        [TestMethod]
        public void Daily_DaysWithoutCollection_AreSkippedAndComparedWithTheLastCollectedDay()
        {
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 12, 20), views: 100),
                    Point(Brasilia(2026, 10, 15, 20), views: 160))
                .Build();

            var daily = CampaignInsightsCalculator.Calculate(data).Daily;

            Assert.AreSequenceEqual(new[] { Oct12, Oct15 }, daily.Select(day => day.Date));
            Assert.AreSequenceEqual(new[] { 100, 60 }, daily.Select(day => day.Views));
        }

        [TestMethod]
        public void Daily_IsSortedByDate()
        {
            var data = Campaign()
                .WithPublication(Point(Brasilia(2026, 10, 14, 20), views: 10))
                .WithPublication(Point(Brasilia(2026, 10, 12, 20), views: 20))
                .Build();

            var daily = CampaignInsightsCalculator.Calculate(data).Daily;

            Assert.AreSequenceEqual(new[] { Oct12, Oct14 }, daily.Select(day => day.Date));
        }

        [TestMethod]
        public void Daily_WhenMetricDecreases_GainIsZero()
        {
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 12, 20), views: 100),
                    Point(Brasilia(2026, 10, 13, 20), views: 90))
                .Build();

            var daily = CampaignInsightsCalculator.Calculate(data).Daily;

            Assert.AreSequenceEqual(new[] { 100, 0 }, daily.Select(day => day.Views));
        }

        [TestMethod]
        public void Daily_WhenMetricRecoversAfterDecrease_DoesNotCountItTwice()
        {
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 12, 20), views: 100),
                    Point(Brasilia(2026, 10, 13, 20), views: 90),
                    Point(Brasilia(2026, 10, 14, 20), views: 100))
                .Build();

            var daily = CampaignInsightsCalculator.Calculate(data).Daily;

            Assert.AreSequenceEqual(new[] { 100, 0, 0 }, daily.Select(day => day.Views));
        }

        [TestMethod]
        public void Daily_WhenMetricIsMissingInADay_DoesNotCountItTwice()
        {
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 12, 20), views: 100),
                    Point(Brasilia(2026, 10, 13, 20), views: null),
                    Point(Brasilia(2026, 10, 14, 20), views: 150))
                .Build();

            var daily = CampaignInsightsCalculator.Calculate(data).Daily;

            Assert.AreSequenceEqual(new[] { 100, 0, 50 }, daily.Select(day => day.Views));
        }

        [TestMethod]
        public void Daily_SumOfGains_EqualsLatestTotal()
        {
            var data = Campaign()
                .WithPublication(
                    Point(Brasilia(2026, 10, 12, 20), views: 120),
                    Point(Brasilia(2026, 10, 13, 20), views: 480),
                    Point(Brasilia(2026, 10, 14, 20), views: 510))
                .WithPublication(
                    Point(Brasilia(2026, 10, 13, 9), views: 40),
                    Point(Brasilia(2026, 10, 14, 9), views: 95))
                .Build();

            var response = CampaignInsightsCalculator.Calculate(data);

            Assert.AreEqual(response.Totals.Views, response.Daily.Sum(day => day.Views));
        }

        #endregion
    }
}
