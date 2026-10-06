namespace Sienna.Domain.Abstractions.Identity.DTOs
{
    /// <summary>Time do qual o usuário participa.</summary>
    public class UserTeamDTO
    {
        /// <summary>ID do time.</summary>
        public Guid TeamId { get; set; }
        /// <summary>Nome do time.</summary>
        public string TeamName { get; set; } = string.Empty;
        /// <summary>Papel do usuário no time (dono, administrador ou membro).</summary>
        public string Role { get; set; } = string.Empty;
    }
}
