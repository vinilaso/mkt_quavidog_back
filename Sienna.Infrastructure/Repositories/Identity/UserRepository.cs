using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Identity;
using Sienna.Domain.Entities.Identity;

namespace Sienna.Infrastructure.Repositories.Identity
{
    internal class UserRepository(ApplicationContext context) : IUserRepository
    {
        public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.Set<User>()
                .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
        }
    }
}
