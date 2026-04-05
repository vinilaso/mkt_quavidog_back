using System.Text.Json.Serialization;

namespace Sienna.Infrastructure.Email.MailerSend
{
    internal record MailerSendPayload(
        [property: JsonPropertyName("from")] MailerSendAddress From,
        [property: JsonPropertyName("to")] MailerSendAddress[] To,
        [property: JsonPropertyName("subject")] string Subject,
        [property: JsonPropertyName("text")] string? PlainBody = default,
        [property: JsonPropertyName("html")] string? HTMLBody = default,
        [property: JsonPropertyName("attachments")] MailerSendAttachment[]? Attachments = default
    );

    internal record MailerSendAddress(
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("name")] string? Name = null
    );

    internal record MailerSendAttachment(
        [property: JsonPropertyName("content")] string Content,
        [property: JsonPropertyName("filename")] string FileName,
        [property: JsonPropertyName("disposition")] string Disposition,
        [property: JsonPropertyName("id")] string? IdHTML = default  
    );
}
