using Microsoft.AspNetCore.Identity;
using Sienna.Domain.Abstractions.Identity;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Infrastructure.Authentication
{
    public sealed class IdentityService(UserManager<User> userManager, SignInManager<User> signInManager) : IIdentityService
    {
        public async Task<AuthenticationResult> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return new AuthenticationResult(AuthenticationStatus.InvalidCredentials);

            var result = await signInManager.CheckPasswordSignInAsync(user, password, true);

            if (result.Succeeded)
                return new AuthenticationResult(AuthenticationStatus.Success, user);

            if (result.IsLockedOut)
                return new AuthenticationResult(AuthenticationStatus.LockedOut, user);

            return new AuthenticationResult(AuthenticationStatus.InvalidCredentials);
        }

        public async Task<Result> ChangePasswordAsync(string email, string token, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Error.NotFound("Email.NotFound", "O e-mail não foi encontrado na base de dados.");

            var result = await userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
                return Result.Success();

            var blockingError = result.Errors.First();
            return Error.Validation(blockingError.Code, blockingError.Description);
        }

        public async Task<Result<string>> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
                return Error.NotFound("Email.NotFound", "O e-mail não foi encontrado na base de dados.");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrWhiteSpace(token))
                return Error.Failure("PasswordReset.Failure", "Ocorreu um erro ao gerar o token de redefinição de senha.");

            return token;
        }

        public async Task<Result<Guid>> RegisterUserAsync(string email, string fullName, string password, CancellationToken cancellationToken = default)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Email = email,
                UserName = email
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
                return Result.Success(user.Id);

            var blockingError = result.Errors.First();

            return blockingError.Code switch
            {
                nameof(IdentityErrorDescriber.DuplicateUserName) or
                nameof(IdentityErrorDescriber.DuplicateEmail) or
                nameof(IdentityErrorDescriber.ConcurrencyFailure) => Error.Conflict(blockingError.Code, blockingError.Description),

                _ => Error.Validation(blockingError.Code, blockingError.Description)
            };
        }
    }
}
