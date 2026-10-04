using Sienna.Application.Messaging.Teams;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.Behaviors.Teams
{
    internal sealed class TeamAccessContext : ITeamAccessContext
    {
        private Guid? _teamId;
        private TeamMemberRole _role;

        public Guid TeamId => _teamId ?? throw NotInitialized();

        public TeamMemberRole Role => _teamId.HasValue ? _role : throw NotInitialized();

        public bool IsManager => Role.IsManager();

        internal void Set(Guid teamId, TeamMemberRole role)
        {
            _teamId = teamId;
            _role = role;
        }

        private static InvalidOperationException NotInitialized() =>
            new("O acesso ao time não foi verificado. A requisição implementa ITeamScopedRequest?");
    }
}
