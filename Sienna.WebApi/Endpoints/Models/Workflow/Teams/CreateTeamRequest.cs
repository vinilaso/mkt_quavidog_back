using Sienna.Application.UseCases.Workflow.CreateTeam;

namespace Sienna.WebApi.Endpoints.Models.Workflow.Teams
{
    /// <summary>Dados para criar um time.</summary>
    /// <param name="TeamName">Nome do time.</param>
    public record CreateTeamRequest(string TeamName)
    {
        public CreateTeamCommand ToCommand() => new(TeamName);
    }
}
