using Sienna.Domain.Entities.Workflow;

namespace Sienna.WebApi.Endpoints.Models.Workflow.Teams
{
    /// <param name="UserId">ID do usuário a ser adicionado ao time.</param>
    /// <param name="Role">Papel do usuário no time: "member" ou "administrator".</param>
    public record AssignUserToTeamRequest(Guid UserId, string Role)
    {
        /// <summary>
        /// Converte o papel informado no enum do domínio. Retorna null se o valor não for aceito.
        /// </summary>
        public TeamMemberRole? ToTeamMemberRole()
        {
            return Role?.Trim().ToLowerInvariant() switch
            {
                "member" => TeamMemberRole.Member,
                "administrator" => TeamMemberRole.Administrator,
                _ => null
            };
        }
    }
}
