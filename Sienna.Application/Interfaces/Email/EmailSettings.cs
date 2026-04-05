namespace Sienna.Application.Interfaces.Email
{
    public record EmailSettings(string SenderEmail, string? SenderName = default);
}
