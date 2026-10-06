namespace Sienna.Domain.Abstractions.Identity.DTOs
{
    /// <summary>Times de um usuário.</summary>
    public class UserTeamsDTO
    {
        /// <summary>ID do usuário.</summary>
        public Guid UserId { get; set; }
        /// <summary>Times dos quais o usuário participa.</summary>
        public IEnumerable<UserTeamDTO> Teams { get; set; } = [];
    }
}
