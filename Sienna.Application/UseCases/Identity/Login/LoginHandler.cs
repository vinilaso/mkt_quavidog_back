using MediatR;
using Microsoft.AspNetCore.Identity;
using Sienna.Application.Interfaces;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Application.UseCases.Identity.Login
{
    public sealed class LoginHandler(UserManager<User> userManager, ITokenService tokenService) : IRequestHandler<LoginCommand, Result<string>>
    {
        private static readonly Error UnauthorizedError = Error.Unauthorized("InvalidCredentials", "Email or password is invalid.");

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null) 
                return UnauthorizedError;

            if (await userManager.IsLockedOutAsync(user))
                return UnauthorizedError;

            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                await userManager.AccessFailedAsync(user);
                return UnauthorizedError;
            }

            await userManager.ResetAccessFailedCountAsync(user);
            return tokenService.GenerateToken(user);
        }
    }
}
