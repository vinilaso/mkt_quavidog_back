using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Entities.Media;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Domain.Entities.Social
{
    public class PostPublication : IDbEntity
    {
        public const int MaxFeedMedia = 10;
        public const int MaxStoryMedia = 10;
        public const int MaxRejectionReasonLength = 500;
        public const int MaxFailureReasonLength = 2000;

        public static readonly TimeSpan MaxScheduleAhead = TimeSpan.FromDays(30);
        private static readonly TimeSpan PastTolerance = TimeSpan.FromMinutes(1);

        public Guid Id { get; set; }

        public Guid PostId { get; private set; }
        public Post? Post { get; private set; }

        public Guid TeamId { get; private set; }
        public Team? Team { get; private set; }

        public PublicationFormat Format { get; private set; }
        public PublicationStatus Status { get; private set; }

        public DateTime ScheduledFor { get; private set; }
        
        public Guid RequestedById { get; private set; }
        public DateTime RequestedAt { get; private set; }

        public Guid? ReviewedById { get; private set; }
        public DateTime? ReviewedAt { get; private set; }
        public string? RejectionReason { get; private set; }

        public DateTime? PublishedAt { get; private set; }

        public string[] ExternalIds { get; private set; } = [];
        public string? FailureReason { get; private set; }

        public bool BlocksNewRequests =>
            Status is PublicationStatus.PendingApproval
                or PublicationStatus.Approved
                or PublicationStatus.Publishing
                or PublicationStatus.Published;

        protected PostPublication()
        {
        }

        #region Solicitação

        public static Result<PostPublication> Request(PublicationRequest request, DateTime utcNow)
        {
            ArgumentNullException.ThrowIfNull(request);

            var mediaResult = ValidateMediaCount(request.Format, request.MediaCount);

            if (mediaResult.IsFailure)
                return mediaResult.Error;

            var when = request.ScheduledFor ?? utcNow;
            var scheduleResult = ValidateSchedule(when, utcNow);

            if (scheduleResult.IsFailure)
                return scheduleResult.Error;

            var publication = new PostPublication
            {
                Id = Guid.NewGuid(),
                PostId = request.PostId,
                TeamId = request.TeamId,
                Format = request.Format,
                ScheduledFor = when,
                RequestedById = request.RequestedById,
                RequestedAt = utcNow,
                Status = PublicationStatus.PendingApproval
            };

            if (request.RequesterIsManager)
                publication.MarkAsApproved(request.RequestedById, utcNow);

            return publication;
        }

        #endregion

        #region Revisão

        public Result Approve(Guid reviewerId, DateTime utcNow)
        {
            if (Status is not PublicationStatus.PendingApproval)
                return InvalidTransition("aprovar");

            MarkAsApproved(reviewerId, utcNow);
            return Result.Success();
        }

        public Result Reject(Guid reviewerId, string reason, DateTime utcNow)
        {
            if (Status is not PublicationStatus.PendingApproval)
                return InvalidTransition("reprovar");

            reason = reason?.Trim() ?? string.Empty;

            if (reason.Length == 0)
                return Error.Validation("Publication.RejectReasonRequired", "Informe o motivo da reprovação.");

            if (reason.Length > MaxRejectionReasonLength)
                return Error.Validation("Publication.RejectReasonTooLong", $"O motivo de reprovação deve ter no máximo {MaxRejectionReasonLength} caracteres.");

            MarkAsRejected(reviewerId, reason, utcNow);
            return Result.Success();
        }

        private void MarkAsApproved(Guid reviewerId, DateTime utcNow)
        {
            Status = PublicationStatus.Approved;
            ReviewedById = reviewerId;
            ReviewedAt = utcNow;
            RejectionReason = null;
        }

        private void MarkAsRejected(Guid reviewerId, string reason, DateTime utcNow)
        {
            Status = PublicationStatus.Rejected;
            ReviewedById = reviewerId;
            ReviewedAt = utcNow;
            RejectionReason = reason;
        }

        #endregion

        #region Alterações antes da publicação

        public Result Cancel()
        {
            if (Status is not (PublicationStatus.PendingApproval or PublicationStatus.Approved))
                return InvalidTransition("cancelar");

            Status = PublicationStatus.Canceled;
            return Result.Success();
        }

        public Result Reschedule(DateTime newScheduledFor, bool byManager, DateTime utcNow)
        {
            if (Status is not (PublicationStatus.PendingApproval or PublicationStatus.Approved))
                return InvalidTransition("reagendar");

            var scheduleResult = ValidateSchedule(newScheduledFor, utcNow);

            if (scheduleResult.IsFailure)
                return scheduleResult.Error;

            ScheduledFor = newScheduledFor;

            if (Status is PublicationStatus.Approved && !byManager)
            {
                Status = PublicationStatus.PendingApproval;
                ReviewedById = null;
                ReviewedAt = null;
            }

            return Result.Success();
        }

        #endregion

        #region Resultado da publicação

        public Result MarkAsPublished(IReadOnlyCollection<string> externalIds, DateTime utcNow)
        {
            if (Status is not PublicationStatus.Publishing)
                return InvalidTransition("concluir");

            if (externalIds.Count == 0)
                return Error.Validation("Publication.RequiresExternalIds", "Uma publicação concluída precisa ter ao menos um ID da rede social.");

            Status = PublicationStatus.Published;
            ExternalIds = [.. externalIds];
            PublishedAt = utcNow;
            FailureReason = null;

            return Result.Success();
        }

        public Result MarkAsFailed(string reason)
        {
            if (Status is not PublicationStatus.Publishing)
                return InvalidTransition("registrar falha de");

            reason = string.IsNullOrWhiteSpace(reason) ? "Falha desconhecida ao publicar." : reason;

            Status = PublicationStatus.Failed;
            FailureReason = reason.Length > MaxFailureReasonLength
                ? reason[..MaxFailureReasonLength]
                : reason;

            return Result.Success();
        }

        public Result Retry(DateTime utcNow)
        {
            if (Status is not PublicationStatus.Failed)
                return InvalidTransition("reenviar");

            Status = PublicationStatus.Approved;
            ScheduledFor = utcNow;
            FailureReason = null;

            return Result.Success();
        }

        #endregion

        #region Regras

        private static Result ValidateMediaCount(PublicationFormat format, int mediaCount)
        {
            var max = format is PublicationFormat.Story ? MaxStoryMedia : MaxFeedMedia;

            if (mediaCount < 1 || mediaCount > max)
                return Error.Validation("Publication.InvalidMediaCount", $"A postagem precisa ter entre 1 e {max} imagens para este formato.");

            return Result.Success();
        }

        private static Result ValidateSchedule(DateTime scheduledFor, DateTime utcNow)
        {
            if (scheduledFor.Kind is not DateTimeKind.Utc)
                return Error.Validation("ScheduledFor.NotUTC", "O horário do agendamento deve estar em UTC.");

            if (scheduledFor < utcNow - PastTolerance)
                return Error.Validation("Publication.ScheduleInPast", "O horário do agendamento não pode estar no passado.");

            if (scheduledFor > utcNow + MaxScheduleAhead)
                return Error.Validation("Publication.ScheduleTooFar", $"O agendamento pode ser feito com no máximo {MaxScheduleAhead.Days} dias de antecedência.");

            return Result.Success();
        }

        private Error InvalidTransition(string action)
        {
            return Error.Conflict("Publication.InvalidStatus", $"Não é possível {action} uma publicação com status {Status}.");
        }

        #endregion
    }
}
