using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions;

namespace Sienna.Infrastructure.Repositories
{
    internal abstract class AbstractRepository<T>(ApplicationContext applicationContext) : IAbstractRepository<T> where T : class, IDbEntity
    {
        protected readonly ApplicationContext Context = applicationContext;

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await Context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public virtual async Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
    }
}
