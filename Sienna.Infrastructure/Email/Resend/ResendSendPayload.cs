using System.Text.Json.Serialization;

namespace Sienna.Infrastructure.Email.Resend
{
    internal record ResendSendPayload(
        [property: JsonPropertyName("from")] string From,
        [property: JsonPropertyName("to")] string[] To,
        [property: JsonPropertyName("subject")] string Subject,
        [property: JsonPropertyName("html")] string? Html = default,
        [property: JsonPropertyName("text")] string? Text = default
    );
}
