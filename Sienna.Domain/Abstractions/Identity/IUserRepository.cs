using Sienna.Domain.Entities.Identity;

namespace Sienna.Domain.Abstractions.Identity
{
    public interface IUserRepository
    {
        Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
