using Sienna.Application.UseCases.Workflow.AssignUserToTeam;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.WebApi.Endpoints.Models.Workflow.Teams
{
    /// <param name="UserId">ID do usuário a ser adicionado ao time.</param>
    /// <param name="Role">Papel do usuário no time: "member" ou "administrator".</param>
    public record AssignUserToTeamRequest(Guid UserId, string Role)
    {
        public static class Roles
        {
            public const string Member = "member";
            public const string Administrator = "administrator";
        }

        public TeamMemberRole? ToTeamMemberRole()
        {
            return Role?.Trim().ToLowerInvariant() switch
            {
                Roles.Member => TeamMemberRole.Member,
                Roles.Administrator => TeamMemberRole.Administrator,
                _ => null
            };
        }

        public Result<AssignUserToTeamCommand> ToCommand(Guid teamId)
        {
            var role = ToTeamMemberRole();

            if (role is null)
                return Error.Validation("Team.InvalidRole", $"O papel deve ser \"{Roles.Member}\" ou \"{Roles.Administrator}\".");

            return new AssignUserToTeamCommand(teamId, UserId, role.Value);
        }
    }
}
