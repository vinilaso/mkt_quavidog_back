using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.DTOs.Invitations
{
    /// <summary>
    /// Dados para o envio do e-mail de convite.
    /// </summary>
    public class InvitationEmailDTO
    {
        public string Email { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string InvitedByName { get; set; } = string.Empty;
        public TeamMemberRole Role { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
