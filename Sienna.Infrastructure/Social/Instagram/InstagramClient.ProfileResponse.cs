using System.Text.Json.Serialization;

namespace Sienna.Infrastructure.Social.Instagram
{
    public sealed partial class InstagramClient
    {
        private record ProfileResponse(
            [property: JsonPropertyName("user_id")] string UserId,
            [property: JsonPropertyName("username")] string Username,
            [property: JsonPropertyName("account_type")] string AccountType
        );
    }
}
