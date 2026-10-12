using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Abstractions.Workflow.DTOs.Invitations
{
    /// <summary>
    /// Convite pendente de um time, na visão do gestor.
    /// </summary>
    public class TeamInvitationDTO
    {
        /// <summary>
        /// ID do convite.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// E-mail convidado.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Papel oferecido.
        /// </summary>
        public TeamMemberRole Role { get; set; }

        /// <summary>
        /// Nome de quem convidou.
        /// </summary>
        public string InvitedByName { get; set; } = string.Empty;

        /// <summary>
        /// Quando o convite foi enviado, em UTC.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Quando o convite expira, em UTC.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
