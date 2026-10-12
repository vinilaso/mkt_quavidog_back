using System.Text.Json.Serialization;

namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed partial class InstagramClient
    {
        private sealed record InsightMetricResponse(
            [property: JsonPropertyName("name")] string Name,
            [property: JsonPropertyName("values")] IReadOnlyList<InsightValueResponse>? Values,
            [property: JsonPropertyName("total_value")] InsightValueResponse? TotalValue
        );
    }
}
