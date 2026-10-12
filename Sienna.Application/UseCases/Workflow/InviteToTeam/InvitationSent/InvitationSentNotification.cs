using MediatR;

namespace Sienna.Application.UseCases.Workflow.InviteToTeam.InvitationSent
{
    internal sealed record InvitationSentNotification(Guid InvitationId) : INotification;
}
