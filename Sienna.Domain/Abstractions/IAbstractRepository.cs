namespace Sienna.Domain.Abstractions
{
    public interface IAbstractRepository<T>
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
