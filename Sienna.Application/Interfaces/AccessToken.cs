namespace Sienna.Application.Interfaces
{
    public record AccessToken(string Token, DateTime ExpiresAt, long ExpiresInMilliseconds);
}
