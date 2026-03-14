using MediatR;
using Microsoft.AspNetCore.Identity;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Application.UseCases.Identity.RegisterUser
{
    public sealed class RegisterUserHandler(UserManager<User> userManager) : IRequestHandler<RegisterUserCommand, Guid>
    {
        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

            return user.Id;
        }
    }
}
