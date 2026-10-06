namespace Sienna.Domain.Abstractions.Workflow.DTOs.Teams
{
    /// <summary>Membros de um time.</summary>
    public class TeamMembersDTO
    {
        /// <summary>ID do time.</summary>
        public Guid TeamId { get; set; }
        /// <summary>Nome do time.</summary>
        public string TeamName { get; set; } = string.Empty;
        /// <summary>Membros, ordenados por papel (dono, administradores e membros) e depois por nome.</summary>
        public IEnumerable<TeamMemberDTO> Members { get; set; } = [];
    }
}
