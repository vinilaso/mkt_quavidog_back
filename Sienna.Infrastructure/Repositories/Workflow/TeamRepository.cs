using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Infrastructure.Repositories.Workflow
{
    internal class TeamRepository(ApplicationContext context) : AbstractRepository<Team>(context), ITeamRepository
    {
    }
}
