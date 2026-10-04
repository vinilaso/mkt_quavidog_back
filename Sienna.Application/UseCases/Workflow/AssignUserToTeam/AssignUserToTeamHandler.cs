using MediatR;
using Sienna.Application.UseCases.Workflow.AssignUserToTeam.UserAssigned;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Identity;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.AssignUserToTeam
{
    public sealed class AssignUserToTeamHandler(
        ITeamRepository teamRepository,
        IUserRepository userRepository,
        IUserContext userContext,
        IUnitOfWork uow,
        IPublisher publisher) : IRequestHandler<AssignUserToTeamCommand, Result>
    {
        public async Task<Result> Handle(AssignUserToTeamCommand request, CancellationToken cancellationToken)
        {
            if (await teamRepository.FindByIdAsync(request.TeamId, cancellationToken) is not Team team)
                return Error.NotFound("Team.NotFound", "Não foi encontrado um time com o ID informado.");

            if (await userRepository.FindByIdAsync(request.UserId, cancellationToken) is not User user)
                return Error.NotFound("User.NotFound", "Não foi encontrado um usuário com o ID informado.");

            var result = TryAssignUser(team, user, request.Role);

            if (result.IsSuccess)
            {
                await uow.CommitChangesAsync(cancellationToken);
                await publisher.Publish(new UserAssignedNotification(team.Name, userContext.Name, user.Email!, request.Role), cancellationToken);
            }

            return result;
        }

        private static Result TryAssignUser(Team team, User user, TeamMemberRole role)
        {
            return role switch
            {
                TeamMemberRole.Administrator => team.TryAddAdmin(user.Id),
                TeamMemberRole.Member => team.TryAddMember(user.Id),
                _ => Error.Validation("Team.InvalidRole", "Não é possível associar usuários ao time com este papel.")
            };
        }
    }
}
