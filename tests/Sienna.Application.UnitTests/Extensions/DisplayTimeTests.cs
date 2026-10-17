using Sienna.Application.Extensions;
using System.Globalization;

namespace Sienna.Application.UnitTests.Extensions
{
    [TestClass]
    public sealed class DisplayTimeTests
    {
        [TestMethod]
        [DataRow("2026-10-13T02:59:00Z", "2026-10-12")] // 12/10 às 23:59 em Brasília
        [DataRow("2026-10-13T03:00:00Z", "2026-10-13")] // 13/10 à 00:00 em Brasília
        [DataRow("2026-10-13T15:00:00Z", "2026-10-13")]
        public void ToBrazilDate_UsesBrasiliaTimeZone(string utc, string expectedDate)
        {
            var utcTime = DateTime.Parse(utc, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

            Assert.AreEqual(DateOnly.Parse(expectedDate, CultureInfo.InvariantCulture), DisplayTime.ToBrazilDate(utcTime));
        }
    }
}
