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
            try
            {
                int rowsAffected = await context.SaveChangesAsync(cancellationToken);
                return rowsAffected > 0;
            }
            catch (DbUpdateException e) when (e.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } postgresException)
            {
                throw new DuplicateEntryException(postgresException.ConstraintName, e);
            }
        }
    }
}
