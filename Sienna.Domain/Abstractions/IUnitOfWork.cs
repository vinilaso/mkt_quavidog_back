namespace Sienna.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task<bool> CommitChangesAsync(CancellationToken cancellationToken = default);
    }
}
