using Microsoft.AspNetCore.Identity;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Entities.Identity
{
    public class User : IdentityUser<Guid>, IDbEntity
    {
        private readonly List<TeamMember> _teams = [];

        public string FullName { get; set; } = string.Empty;
        public IReadOnlyCollection<TeamMember> Teams => _teams.AsReadOnly();

        public User(string fullName, string email)
        {
            Id = Guid.NewGuid();
            Email = email;
            FullName = fullName;
            UserName = email;
        }

        protected User()
        {
        }
    }
}
