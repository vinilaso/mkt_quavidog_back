using System.Text.Json.Serialization;

namespace Sienna.Infrastructure.Email.Resend
{
    internal record ResendSendPayload(
        [property: JsonPropertyName("from")] string From,
        [property: JsonPropertyName("to")] string[] To,
        [property: JsonPropertyName("subject")] string Subject,
        [property: JsonPropertyName("html"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Html = default,
        [property: JsonPropertyName("text"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? Text = default,
        [property: JsonPropertyName("template"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] ResendMailTemplate? MailTemplate = default
    );

    internal record ResendMailTemplate(
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("variables")] object Variables
    );
}
