using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.AssignUserToTeam.UserAssigned
{
    public sealed class UserAssignedNotificationHandler(IEmailQueue emailQueue) : INotificationHandler<UserAssignedNotification>
    {
        public async Task Handle(UserAssignedNotification notification, CancellationToken cancellationToken)
        {
            var role = notification.Role is TeamMemberRole.Administrator ? "administrador" : "membro";

            var message = new MailMessageBuilder()
                .AddSubject("Você foi associado a um time.")
                .AddPlainBody($"O usuário {notification.AssignedBy} adicionou você ao time {notification.TeamName} como {role}.")
                .AddRecipient(notification.UserAssignedEmail)
                .Build();

            await emailQueue.EnqueueAsync(message, cancellationToken);
        }
    }
}
