using MediatR;
using Microsoft.Extensions.Logging;
using Sienna.Application.Extensions;
using Sienna.Application.UseCases.Workflow.InviteToTeam.InvitationSent;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Security;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;
using Sienna.Domain.Exceptions.Persistence;
using Sienna.Shared.Rules;

namespace Sienna.Application.UseCases.Workflow.InviteToTeam
{
    public sealed class InviteToTeamHandler(
        IUserContext userContext,
        ITeamRepository teamRepository,
        ITeamInvitationRepository invitationRepository,
        IPublisher publisher,
        IUnitOfWork uow,
        ILogger<InviteToTeamHandler> logger) : IRequestHandler<InviteToTeamCommand, Result>
    {
        public async Task<Result> Handle(InviteToTeamCommand request, CancellationToken cancellationToken)
        {
            var normalizedEmail = EmailRules.Normalize(request.Email);

            if (await teamRepository.HasMemberWithEmailAsync(request.TeamId, normalizedEmail, cancellationToken))
                return InvitationErrors.AlreadyMember;

            var invitationResult = await GetInvitationAsync(request, normalizedEmail, cancellationToken);

            if (invitationResult.IsFailure)
                return invitationResult.Error;

            try
            {
                await uow.CommitChangesAsync(cancellationToken);
            }
            catch (DuplicateEntryException)
            {
                // Dois envios simultâneos para o mesmo e-mail. Se entrar, o outro processo já criou o convite e enviou o e-mail.
                return Result.Success();
            }

            await publisher.PublishSafelyAsync(new InvitationSentNotification(invitationResult.Value.Id), logger, cancellationToken);
            return Result.Success();
        }

        private async Task<Result<TeamInvitation>> GetInvitationAsync(InviteToTeamCommand request, string normalizedEmail, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            if (await invitationRepository.FindPendingAsync(request.TeamId, normalizedEmail, cancellationToken) is TeamInvitation pending)
                return TryRenewExistingInvitation(pending, request.Role, now);

            return await CreateNewInvitationAsync(request, normalizedEmail, now, cancellationToken);
        }

        private Result<TeamInvitation> TryRenewExistingInvitation(TeamInvitation invitation, TeamMemberRole role, DateTime now)
        {
            var renewed = invitation.Renew(new RenewInvitationRequest
            {
                InvitedById = userContext.Id,
                Role = role,
                UtcNow = now
            });

            if (renewed.IsFailure)
                return renewed.Error;

            return invitation;
        }

        private async Task<Result<TeamInvitation>> CreateNewInvitationAsync(InviteToTeamCommand request, string normalizedEmail, DateTime now, CancellationToken cancellationToken)
        {
            var created = TeamInvitation.Create(new TeamInvitationRequest
            {
                Email = normalizedEmail,
                InvitedById = userContext.Id,
                Role = request.Role,
                TeamId = request.TeamId,
                UtcNow = now
            });

            if (created.IsFailure)
                return created.Error;

            await invitationRepository.AddAsync(created.Value, cancellationToken);
            return created.Value;
        }
    }
}
