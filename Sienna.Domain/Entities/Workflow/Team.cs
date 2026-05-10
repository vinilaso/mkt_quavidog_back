using Sienna.Domain.Abstractions.Results;

namespace Sienna.Domain.Entities.Workflow
{
    public class Team
    {
        private readonly List<TeamMember> _members = [];

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public IReadOnlyCollection<TeamMember> Members => _members.AsReadOnly();

        public Team(string name, Guid ownerId)
        {
            Name = name;
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;

            TryAddUserWithRole(ownerId, TeamMemberRole.Owner);
        }

        protected Team()
        {
        }

        public Result TryAddMember(Guid userId)
        {
            return TryAddUserWithRole(userId, TeamMemberRole.Member);
        }

        public Result TryAddAdmin(Guid userId)
        {
            return TryAddUserWithRole(userId, TeamMemberRole.Administrator);
        }

        private Result TryAddUserWithRole(Guid userId, TeamMemberRole role)
        {
            if (_members.Exists(m => m.MemberId == userId))
                return Error.Conflict("TeamMember.DuplicateUser", $"O usuário já pertence ao time {Name}.");

            _members.Add(new TeamMember(Id, userId, role));
            return Result.Success();
        }
    }
}
