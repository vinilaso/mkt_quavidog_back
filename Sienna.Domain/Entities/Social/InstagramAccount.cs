using Sienna.Domain.Abstractions;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Entities.Social
{
    public class InstagramAccount : IDbEntity
    {
        public Guid Id { get; set; }
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

        public string InstagramUserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string ProtectedAccessToken { get; set; } = string.Empty;

        public DateTime ConnectedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        protected InstagramAccount()
        {
        }

        public InstagramAccount(Guid teamId, string instagramUserId, string username, string protectedAccessToken)
        {
            Id = Guid.NewGuid();
            TeamId = teamId;
            ConnectedAt = DateTime.UtcNow;
            UpdateCredentials(instagramUserId, username, protectedAccessToken);
        }

        public void UpdateCredentials(string instagramUserId, string username, string protectedAccessToken)
        {
            InstagramUserId = instagramUserId;
            Username = username;
            ProtectedAccessToken = protectedAccessToken;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
