using Sienna.Application.UseCases.Workflow.InviteToTeam;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.WebApi.Endpoints.Models.Workflow.Teams
{
    /// <summary>
    /// Dados para convidar alguém para o time.
    /// </summary>
    /// <param name="Email">E-mail da pessoa convidada. Ela precisa entrar (ou se cadastrar) com este e-mail para aceitar.</param>
    /// <param name="Role">Papel oferecido: member ou administrator.</param>
    public sealed record InviteToTeamRequest(string Email, TeamMemberRole Role)
    {
        public InviteToTeamCommand ToCommand(Guid teamId) => new(teamId, Email, Role);
    }
}
