using Microsoft.EntityFrameworkCore;
using Sienna.Domain.Abstractions.Workflow.DTOs.Invitations;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Identity;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Infrastructure.Repositories.Workflow
{
    internal sealed class TeamInvitationRepository(ApplicationContext context) : AbstractRepository<TeamInvitation>(context), ITeamInvitationRepository
    {
        public async Task<TeamInvitation?> FindPendingAsync(Guid teamId, string normalizedEmail, CancellationToken cancellationToken = default)
        {
            return await Context.Set<TeamInvitation>()
                .FirstOrDefaultAsync(invitation =>
                    invitation.TeamId == teamId &&
                    invitation.Email == normalizedEmail &&
                    invitation.Status == InvitationStatus.Pending,
                    cancellationToken);
        }

        public async Task<TeamInvitation?> FindTeamInvitationAsync(Guid teamId, Guid invitationId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<TeamInvitation>()
                .FirstOrDefaultAsync(invitation => invitation.Id == invitationId && invitation.TeamId == teamId, cancellationToken);
        }

        public async Task<InvitationEmailDTO?> GetEmailDataAsync(Guid invitationId, CancellationToken cancellationToken = default)
        {
            return await Context.Set<TeamInvitation>()
                .Where(invitation => invitation.Id == invitationId)
                .Select(invitation => new InvitationEmailDTO
                {
                    Email = invitation.Email,
                    TeamName = invitation.Team!.Name,
                    Role = invitation.Role,
                    ExpiresAt = invitation.ExpiresAt,

                    InvitedByName = Context.Set<User>()
                        .Where(user => user.Id == invitation.InvitedById)
                        .Select(user => user.FullName)
                        .FirstOrDefault() ?? string.Empty
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<UserInvitationDTO>> FindPendingByEmailAsync(string normalizedEmail, DateTime utcNow, CancellationToken cancellationToken = default)
        {
            return await Context.Set<TeamInvitation>()
                .Where(invitation => invitation.Email == normalizedEmail && invitation.Status == InvitationStatus.Pending && invitation.ExpiresAt > utcNow)
                .Select(invitation => new UserInvitationDTO
                {
                    Id = invitation.Id,
                    TeamId = invitation.TeamId,
                    TeamName = invitation.Team!.Name,
                    Role = invitation.Role,
                    ExpiresAt = invitation.ExpiresAt,

                    InvitedByName = Context.Set<User>()
                        .Where(user => user.Id == invitation.InvitedById)
                        .Select(user => user.FullName)
                        .FirstOrDefault() ?? string.Empty
                })
                .OrderBy(dto => dto.ExpiresAt)
                .ThenBy(dto => dto.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TeamInvitationDTO>> GetPendingByTeamAsync(Guid teamId, DateTime utcNow, CancellationToken cancellationToken = default)
        {
            return await Context.Set<TeamInvitation>()
                .Where(invitation => invitation.TeamId == teamId && invitation.Status == InvitationStatus.Pending && invitation.ExpiresAt > utcNow)
                .Select(invitation => new TeamInvitationDTO
                {
                    Id = invitation.Id,
                    Email = invitation.Email,
                    Role = invitation.Role,
                    CreatedAt = invitation.CreatedAt,
                    ExpiresAt = invitation.ExpiresAt,

                    InvitedByName = Context.Set<User>()
                        .Where(user => user.Id == invitation.InvitedById)
                        .Select(user => user.FullName)
                        .FirstOrDefault() ?? string.Empty
                })
                .OrderByDescending(dto => dto.CreatedAt)
                .ThenBy(dto => dto.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
