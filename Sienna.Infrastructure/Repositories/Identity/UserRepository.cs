using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Identity.Repositories;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Infrastructure.Repositories.Identity
{
    internal class UserRepository(ApplicationContext context) : IUserRepository
    {
        public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Set<User>()
                .Include(user => user.Teams)
                .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public async Task<UserTeamsDTO?> GetUserTeamsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.Set<User>()
                .Where(user => user.Id == userId)
                .Select(user => new UserTeamsDTO
                {
                    UserId = user.Id,
                    Teams = user.Teams.Select(team => new UserTeamDTO
                    {
                        TeamId = team.Team.Id,
                        TeamName = team.Team.Name,
                        Role = team.Role.ToString()
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
