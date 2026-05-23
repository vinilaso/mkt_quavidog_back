using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Domain.Abstractions.Identity.Repositories
{
    public interface IUserRepository
    {
        Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserTeamsDTO?> GetUserTeamsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
