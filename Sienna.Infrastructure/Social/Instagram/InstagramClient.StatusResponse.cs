using System.Text.Json.Serialization;

namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed partial class InstagramClient
    {
        private record StatusResponse([property: JsonPropertyName("status_code")] string StatusCode);
    }
}
