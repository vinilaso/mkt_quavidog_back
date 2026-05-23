namespace Sienna.Domain.Abstractions.Identity.DTOs
{
    public class UserTeamDTO
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
