using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.CreateTeam
{
    /// <summary>Dados para criar um time.</summary>
    /// <param name="TeamName">Nome do time.</param>
    public record CreateTeamCommand(string TeamName) : IRequest<Result<Guid>>;
}
