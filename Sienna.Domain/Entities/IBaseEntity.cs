namespace Sienna.Domain.Entities
{
    public interface IBaseEntity : IEquatable<IBaseEntity>
    {
        Guid Id { get; set; }
    }
}
