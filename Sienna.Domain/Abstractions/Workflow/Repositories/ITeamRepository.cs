using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.Repositories
{
    public interface ITeamRepository
    {
        Task AddAsync(Team team);
    }
}
