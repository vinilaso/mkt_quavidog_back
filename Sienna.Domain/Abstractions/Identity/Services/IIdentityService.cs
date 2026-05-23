using Sienna.Domain.Abstractions.Identity.Authentication;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Domain.Abstractions.Identity.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<Guid>> RegisterUserAsync(string email, string fullName, string password, CancellationToken cancellationToken = default);
        Task<Result<string>> GeneratePasswordResetTokenAsync(string email);
        Task<Result> ChangePasswordAsync(string email, string token, string newPassword);
    }
}
