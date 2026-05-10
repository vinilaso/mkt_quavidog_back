using MediatR;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Workflow.CreateTeam
{
    public record CreateTeamCommand(string TeamName) : IRequest<Result<Guid>>;
}
