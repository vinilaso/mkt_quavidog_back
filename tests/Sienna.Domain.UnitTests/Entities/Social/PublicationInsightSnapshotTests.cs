using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Social;
using Sienna.Tests.Common.Time;

namespace Sienna.Domain.UnitTests.Entities.Social
{
    [TestClass]
    public sealed class PublicationInsightSnapshotTests
    {
        [TestMethod]
        public void Create_CopiesMetricsAndCollectionTime()
        {
            var publicationId = Guid.NewGuid();
            var metrics = new InsightMetrics { Views = 200, Reach = 100, Likes = 10, TotalInteractions = 15 };

            var snapshot = ResultAssert.Success(PublicationInsightSnapshot.Create(publicationId, metrics, TestTime.Now));

            Assert.AreEqual(publicationId, snapshot.PublicationId);
            Assert.AreEqual(TestTime.Now, snapshot.CollectedAt);
            Assert.AreEqual(200, snapshot.Views);
            Assert.AreEqual(10, snapshot.Likes);
            Assert.IsNull(snapshot.Replies);
            Assert.AreEqual(0.15, snapshot.EngagementRate);
        }

        [TestMethod]
        public void Create_WithNegativeValue_ReturnsValidationError()
        {
            var metrics = new InsightMetrics { Views = 10, Likes = -1 };

            var result = PublicationInsightSnapshot.Create(Guid.NewGuid(), metrics, TestTime.Now);

            ResultAssert.Failure(result, "Insights.NegativeValue", ErrorType.Validation);
        }

        [TestMethod]
        [DataRow(DateTimeKind.Local)]
        [DataRow(DateTimeKind.Unspecified)]
        public void Create_WithNonUtcTime_Throws(DateTimeKind kind)
        {
            var notUtc = DateTime.SpecifyKind(TestTime.Now, kind);

            Assert.ThrowsExactly<ArgumentException>(() => PublicationInsightSnapshot.Create(Guid.NewGuid(), new InsightMetrics(), notUtc));
        }
    }
}
