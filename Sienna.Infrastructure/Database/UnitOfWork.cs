using Sienna.Domain.Abstractions;

namespace Sienna.Infrastructure.Database
{
    internal sealed class UnitOfWork(ApplicationContext context) : IUnitOfWork
    {
        public async Task<bool> CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            int rowsAffected = await context.SaveChangesAsync(cancellationToken);
            return rowsAffected > 0;
        }
    }
}
