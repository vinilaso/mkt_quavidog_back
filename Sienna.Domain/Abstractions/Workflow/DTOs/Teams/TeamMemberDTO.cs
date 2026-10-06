namespace Sienna.Domain.Abstractions.Workflow.DTOs.Teams
{
    /// <summary>Membro de um time.</summary>
    public class TeamMemberDTO
    {
        /// <summary>ID do usuário.</summary>
        public Guid UserId { get; set; }
        /// <summary>Nome completo do usuário.</summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>E-mail do usuário.</summary>
        public string UserEmail { get; set; } = string.Empty;
        /// <summary>Papel no time: "owner", "administrator" ou "member".</summary>
        public string Role { get; set; } = string.Empty;
        /// <summary>Quando entrou no time (UTC).</summary>
        public DateTime JoinedAt { get; set; }
    }
}
