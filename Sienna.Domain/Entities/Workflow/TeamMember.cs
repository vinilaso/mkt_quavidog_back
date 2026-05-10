using Sienna.Domain.Entities.Identity;

namespace Sienna.Domain.Entities.Workflow
{
    public class TeamMember
    {
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

        public Guid? MemberId { get; set; }
        public User? Member { get; set; }

        public DateTime AssociationDate { get; set; }
        public TeamMemberRole Role { get; set; }

        internal TeamMember(Guid teamId, Guid memberId, TeamMemberRole role)
        {
            Role = role;
            TeamId = teamId;
            MemberId = memberId;
            AssociationDate = DateTime.UtcNow;
        }

        protected TeamMember()
        {
        }
    }
}
