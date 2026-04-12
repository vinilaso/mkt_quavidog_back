namespace Sienna.Domain.Abstractions.Identity
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<Guid>> RegisterUserAsync(string email, string fullName, string password, CancellationToken cancellationToken = default);
    }
}
