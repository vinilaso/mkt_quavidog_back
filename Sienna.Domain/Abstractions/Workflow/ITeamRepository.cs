using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team);
    }
}
