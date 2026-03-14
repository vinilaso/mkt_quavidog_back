namespace Sienna.Domain.Entities
{
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is not BaseEntity) return false;

            if (ReferenceEquals(this, obj)) return true;

            return Equals(obj as BaseEntity);
        }

        public bool Equals(IBaseEntity? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity? left, BaseEntity? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BaseEntity? left, BaseEntity? right)
        {
            return !Equals(left, right); 
        }
    }
}
