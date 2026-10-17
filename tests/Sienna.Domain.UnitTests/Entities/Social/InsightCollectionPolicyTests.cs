using Sienna.Domain.Entities.Social;
using Sienna.Tests.Common.Time;

namespace Sienna.Domain.UnitTests.Entities.Social
{
    /// <summary>Documenta as janelas e os intervalos de coleta.</summary>
    [TestClass]
    public sealed class InsightCollectionPolicyTests
    {
        private static readonly DateTime Now = TestTime.Now;

        [TestMethod]
        public void Stories_AreCollectedOnlyDuringTheFirst24Hours()
        {
            Assert.AreEqual(Now.AddHours(-24), InsightCollectionPolicy.GetStoryStart(Now));
        }

        [TestMethod]
        public void Feed_IsCollectedDuring30Days_IntensivelyInTheFirst48Hours()
        {
            Assert.AreEqual(Now.AddDays(-30), InsightCollectionPolicy.GetFeedStart(Now));
            Assert.AreEqual(Now.AddHours(-48), InsightCollectionPolicy.GetIntensiveStart(Now));
        }

        [TestMethod]
        public void Intervals_AreHourlyWhenIntensiveAndDailyOtherwise()
        {
            Assert.AreEqual(Now.AddHours(-1), InsightCollectionPolicy.GetIntensiveDue(Now));
            Assert.AreEqual(Now.AddHours(-24), InsightCollectionPolicy.GetRegularDue(Now));
        }
    }
}
