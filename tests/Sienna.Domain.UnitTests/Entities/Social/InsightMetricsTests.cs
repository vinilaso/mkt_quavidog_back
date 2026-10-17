using Sienna.Domain.Entities.Social;

namespace Sienna.Domain.UnitTests.Entities.Social
{
    [TestClass]
    public sealed class InsightMetricsTests
    {
        [TestMethod]
        public void Sum_IgnoresNullsAndSumsEachMetric()
        {
            var feed = new InsightMetrics { Views = 100, Reach = 80, Saved = 4 };
            var story = new InsightMetrics { Views = 50, Reach = 30, Replies = 2 };

            var sum = InsightMetrics.Sum([feed, story]);

            Assert.AreEqual(150, sum.Views);
            Assert.AreEqual(110, sum.Reach);
            Assert.AreEqual(4, sum.Saved);
            Assert.AreEqual(2, sum.Replies);
        }

        [TestMethod]
        public void Sum_WhenMetricIsNullInEveryItem_ReturnsNull()
        {
            var sum = InsightMetrics.Sum([new InsightMetrics { Views = 10 }, new InsightMetrics { Views = 20 }]);

            Assert.IsNull(sum.Likes);
        }

        [TestMethod]
        public void Sum_OfEmptyList_ReturnsAllMetricsNull()
        {
            Assert.AreEqual(new InsightMetrics(), InsightMetrics.Sum([]));
        }

        [TestMethod]
        [DataRow(50, 100, 0.5)]
        [DataRow(0, 100, 0.0)]
        [DataRow(10, 0, null)]
        [DataRow(null, 100, null)]
        [DataRow(10, null, null)]
        public void CalculateEngagementRate_DividesInteractionsByReach(int? interactions, int? reach, double? expected)
        {
            Assert.AreEqual(expected, InsightMetrics.CalculateEngagementRate(interactions, reach));
        }
    }
}
