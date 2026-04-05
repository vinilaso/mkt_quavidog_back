namespace Sienna.Application.Interfaces.Email
{
    public record MailMessage
    {
        public IReadOnlyCollection<MailAddress> To { get; init; } = [];
        public ICollection<MailAttachment> Attachments { get; } = [];

        public string Subject { get; init; } = string.Empty;
        public string? Body { get; init; }
        public bool IsHTML { get; init; } = false;
    }

    public record MailAddress(
        string Email,
        string? Name = default
    );

    public record MailAttachment
    {
        public string Content { get; init; } = string.Empty;

        public MailAttachmentDisposition Disposition { get; init; }

        public string FileName { get; init; } = string.Empty;

        public string? IdHTML { get; init; }
    }

    public enum MailAttachmentDisposition
    {
        Inline,
        Attachment
    }
}
