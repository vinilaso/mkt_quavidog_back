using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Shared.Rules;
using System.Net.Mail;

namespace Sienna.Domain.Entities.Workflow
{
    public class TeamInvitation : IDbEntity
    {
        public const int MaxEmailLength = 256;
        public static readonly TimeSpan Validity = TimeSpan.FromDays(7);

        public Guid Id { get; set; }

        public Guid TeamId { get; private set; }
        public Team? Team { get; private set; }

        public string Email { get; private set; } = string.Empty;
        public TeamMemberRole Role { get; private set; }
        public InvitationStatus Status { get; private set; }

        public Guid InvitedById { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? RespondedAt { get; private set; }

        protected TeamInvitation()
        {
        }

        public bool IsExpired(DateTime utcNow) => Status is InvitationStatus.Pending && utcNow >= ExpiresAt;
        public bool IsAddressedTo(string email) => Email == EmailRules.Normalize(email);

        public static Result<TeamInvitation> Create(TeamInvitationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var validation = Validate(request.Email, request.Role);

            if (validation.IsFailure)
                return validation.Error;

            return new TeamInvitation
            {
                Id = Guid.NewGuid(),
                TeamId = request.TeamId,
                Email = EmailRules.Normalize(request.Email),
                Role = request.Role,
                Status = InvitationStatus.Pending,
                InvitedById = request.InvitedById,
                CreatedAt = request.UtcNow,
                ExpiresAt = request.UtcNow + Validity
            };
        }

        public Result Renew(RenewInvitationRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (Status is not InvitationStatus.Pending)
                return InvalidStatus("renovar");

            var validation = Validate(Email, request.Role);

            if (validation.IsFailure)
                return validation.Error;

            Role = request.Role;
            InvitedById = request.InvitedById;
            ExpiresAt = request.UtcNow + Validity;

            return Result.Success();
        }

        public Result Accept(string userEmail, DateTime utcNow)
        {
            var check = EnsureCanRespond(userEmail, utcNow);

            if (check.IsFailure)
                return check.Error;

            Status = InvitationStatus.Accepted;
            RespondedAt = utcNow;

            return Result.Success();
        }

        public Result Decline(string userEmail, DateTime utcNow)
        {
            var check = EnsureCanRespond(userEmail, utcNow);

            if (check.IsFailure)
                return check.Error;

            Status = InvitationStatus.Declined;
            RespondedAt = utcNow;

            return Result.Success();
        }

        public Result Cancel(DateTime utcNow)
        {
            if (Status is not InvitationStatus.Pending)
                return InvalidStatus("cancelar");

            Status = InvitationStatus.Canceled;
            RespondedAt = utcNow;

            return Result.Success();
        }

        private Result EnsureCanRespond(string userEmail, DateTime utcNow)
        {
            if (!IsAddressedTo(userEmail))
                return Error.NotFound("Invitation.NotFound", "Não foi encontrado um convite com o ID informado.");

            if (Status is not InvitationStatus.Pending)
                return InvalidStatus("responder");

            if (IsExpired(utcNow))
                return Error.Conflict("Invitation.Expired", "Este convite expirou. Peça ao gestor do time um novo convite.");

            return Result.Success();
        }

        private static Result Validate(string email, TeamMemberRole role)
        {
            if (role is not (TeamMemberRole.Member or TeamMemberRole.Administrator))
                return Error.Validation("Invitation.InvalidRole", "O convite deve ser para membro ou administrador.");

            var normalized = EmailRules.Normalize(email);

            if (normalized.Length is 0 or > MaxEmailLength || !MailAddress.TryCreate(normalized, out _))
                return Error.Validation("Invitation.InvalidEmail", "Informe um e-mail válido.");

            return Result.Success();
        }

        private Error InvalidStatus(string action) =>
            Error.Validation("Invitation.InvalidStatus", $"Não é possível {action} um convite com status {Status}.");
    }
}
