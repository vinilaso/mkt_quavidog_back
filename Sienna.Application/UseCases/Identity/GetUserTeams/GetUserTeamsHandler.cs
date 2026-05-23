using MediatR;
using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.GetUserTeams
{
    public sealed class GetUserTeamsHandler(IUserRepository userRepository) : IRequestHandler<GetUserTeamsQuery, Result<UserTeamsDTO>>
    {
        public async Task<Result<UserTeamsDTO>> Handle(GetUserTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = await userRepository.GetUserTeamsAsync(request.UserId, cancellationToken);

            if (teams is null)
                return Error.NotFound("User.NotFound", $"Não foi encontrado usuário cadastrado com o ID {request.UserId}");

            return teams;
        }
    }
}
