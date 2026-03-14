using Microsoft.AspNetCore.Identity;

namespace Sienna.Domain.Entities.Identity
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public string? FullName { get; set; }

        public bool Equals(IBaseEntity? other)
        {
            if (other is null) return false;
            if (other is not User) return false;

            if (ReferenceEquals(this, other)) return true;

            return Id.Equals(other.Id);
        }
    }
}
