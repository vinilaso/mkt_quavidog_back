using Microsoft.EntityFrameworkCore;
using Npgsql;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Exceptions.Persistence;

namespace Sienna.Infrastructure.Database
{
    internal sealed class UnitOfWork(ApplicationContext context) : IUnitOfWork
    {
        public async Task<bool> CommitChangesAsync(CancellationToken cancellationToken = default)
        {
            RenewConcurrencyStamps();

            try
            {
                int rowsAffected = await context.SaveChangesAsync(cancellationToken);
                return rowsAffected > 0;
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new ConcurrencyConflictException(e);
            }
            catch (DbUpdateException e) when (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } postgresException)
            {
                throw new DuplicateEntryException(postgresException.ConstraintName, e);
            }
        }

        private void RenewConcurrencyStamps()
        {
            foreach (var entry in context.ChangeTracker.Entries<IConcurrencyStamped>())
            {
                if (entry.State is EntityState.Modified)
                {
                    entry.Entity.RenewConcurrencyStamp();
                }
            }
        }
    }
}
