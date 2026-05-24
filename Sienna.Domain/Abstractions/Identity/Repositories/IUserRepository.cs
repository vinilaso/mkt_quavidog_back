using Sienna.Domain.Abstractions.Identity.DTOs;
using Sienna.Domain.Abstractions.Media.DTOs;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Domain.Abstractions.Identity.Repositories
{
    public interface IUserRepository : IAbstractRepository<User>
    {
        Task<UserTeamsDTO?> GetUserTeamsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PostDTO>> GetUserPostsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
