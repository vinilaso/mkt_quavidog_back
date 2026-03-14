using MediatR;
using Microsoft.AspNetCore.Identity;
using Sienna.Application.Interfaces;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Application.UseCases.Identity.Login
{
    public sealed class LoginHandler(UserManager<User> userManager, ITokenService tokenService) : IRequestHandler<LoginCommand, string>
    {
        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User user = await userManager.FindByEmailAsync(request.Email)
                ?? throw GetInvalidCredentialsException();

            if (!await userManager.CheckPasswordAsync(user, request.Password))
                throw GetInvalidCredentialsException();

            return tokenService.GenerateToken(user);
        }

        private static InvalidOperationException GetInvalidCredentialsException()
        {
            return new InvalidOperationException("Invalid credentials.");
        }
    }
}
