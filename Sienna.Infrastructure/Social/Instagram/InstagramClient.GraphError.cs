using System.Text.Json.Serialization;

namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed partial class InstagramClient
    {
        private record GraphError(
            [property: JsonPropertyName("message")] string Message,
            [property: JsonPropertyName("code")] int Code,
            [property: JsonPropertyName("error_user_msg")] string? UserMessage
        );
    }
}
