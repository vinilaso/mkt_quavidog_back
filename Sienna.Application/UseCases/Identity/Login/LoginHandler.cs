using MediatR;
using Sienna.Application.Interfaces;
using Sienna.Application.UseCases.Identity.Login.Events;
using Sienna.Domain.Abstractions.Identity.Authentication;
using Sienna.Domain.Abstractions.Identity.Services;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Application.UseCases.Identity.Login
{
    public sealed class LoginHandler(
        IIdentityService identityService,
        ITokenService tokenService,
        IPublisher publisher) : IRequestHandler<LoginCommand, Result<string>>
    {
        private static readonly Error UnauthorizedError = Error.Unauthorized("InvalidCredentials", "Email or password is invalid.");

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var authenticationResult = await identityService.AuthenticateAsync(request.Email, request.Password, cancellationToken);

            if (authenticationResult.User is not User user)
            {
                return UnauthorizedError;
            }

            return authenticationResult.Status switch
            {
                AuthenticationStatus.Success => tokenService.GenerateToken(user),
                AuthenticationStatus.LockedOut => await PublishLockedOutNotification(user, cancellationToken),
                _ => UnauthorizedError
            };
        }

        private async Task<Result<string>> PublishLockedOutNotification(User user, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                var lockoutNotification = new UserLockedOutNotification(user.Email, user.FullName);
                await publisher.Publish(lockoutNotification, cancellationToken);
            }
            
            return UnauthorizedError;
        }
    }
}
