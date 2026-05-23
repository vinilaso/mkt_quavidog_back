using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Infrastructure.Repositories.Workflow
{
    internal class TeamRepository(ApplicationContext context) : ITeamRepository
    {
        public async Task AddAsync(Team team)
        {
            await context.Set<Team>().AddAsync(team);
        }
    }
}
