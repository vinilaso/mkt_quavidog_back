using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.DTOs.Invitations
{
    /// <summary>
    /// Convite recebido pelo usuário autenticado.
    /// </summary>
    public class UserInvitationDTO
    {
        /// <summary>
        /// ID do convite.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID do time.
        /// </summary>
        public Guid TeamId { get; set; }

        /// <summary>
        /// Nome do time.
        /// </summary>
        public string TeamName { get; set; } = string.Empty;

        /// <summary>
        /// Papel oferecido.
        /// </summary>
        public TeamMemberRole Role { get; set; }

        /// <summary>
        /// Nome de quem convidou.
        /// </summary>
        public string InvitedByName { get; set; } = string.Empty;

        /// <summary>
        /// Quando o convite expira, em UTC.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
