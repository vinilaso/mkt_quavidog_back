using MediatR;
using Sienna.Application.UseCases.Identity.RegisterUser.Events;
using Sienna.Domain.Abstractions.Identity.Services;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.RegisterUser
{
    public sealed class RegisterUserHandler(
        IIdentityService identityService,
        IPublisher publisher) : IRequestHandler<RegisterUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.RegisterUserAsync(request.Email, request.FullName, request.Password, cancellationToken);
            
            if (result.IsSuccess)
            {
                var userRegisteredNotification = new UserRegisteredNotification(request.FullName, request.Email);
                await publisher.Publish(userRegisteredNotification, cancellationToken);
            }

            return result;
        }
    }
}
