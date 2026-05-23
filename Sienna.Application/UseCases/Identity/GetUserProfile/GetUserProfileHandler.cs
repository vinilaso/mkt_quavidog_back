using MediatR;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.GetUserProfile
{
    public class GetUserProfileHandler(IUserRepository userRepository) : IRequestHandler<GetUserProfileQuery, Result<UserProfileResponse>>
    {
        public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

            if (user is null)
                return Error.NotFound("User.NotFound", $"Não foi encontrado usuário cadastrado com o ID {request.UserId}");

            return new UserProfileResponse(user.Id, user.FullName, user.Email ?? string.Empty);
        }
    }
}
