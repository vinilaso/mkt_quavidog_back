using MediatR;
using Microsoft.AspNetCore.Identity;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Application.UseCases.Identity.RegisterUser
{
    public sealed class RegisterUserHandler(UserManager<User> userManager) : IRequestHandler<RegisterUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return user.Id;
            }

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
