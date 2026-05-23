namespace Sienna.Domain.Abstractions.Identity.DTOs
{
    public class UserTeamsDTO
    {
        public Guid UserId { get; set; }
        public IEnumerable<UserTeamDTO> Teams { get; set; } = [];
    }
}
