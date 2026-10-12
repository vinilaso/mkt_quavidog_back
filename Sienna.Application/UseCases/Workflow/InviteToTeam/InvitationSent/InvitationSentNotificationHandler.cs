using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Extensions;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions.Workflow.DTOs.Invitations;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.InviteToTeam.InvitationSent
{
    internal sealed class InvitationSentNotificationHandler(
        ITeamInvitationRepository invitationRepository,
        IEmailQueue emailQueue) : INotificationHandler<InvitationSentNotification>
    {
        public async Task Handle(InvitationSentNotification notification, CancellationToken cancellationToken)
        {
            if (await invitationRepository.GetEmailDataAsync(notification.InvitationId, cancellationToken) is not InvitationEmailDTO data)
                return;

            var role = data.Role is TeamMemberRole.Administrator ? "administrador" : "membro";

            var message = new MailMessageBuilder()
                .AddRecipient(data.Email)
                .AddSubject($"Convite para o time {data.TeamName}")
                .AddPlainBody($"""
                    Olá!

                    {data.InvitedByName} convidou você para participar do time {data.TeamName}, como {role}.

                    Para aceitar, entre no sistema (ou crie uma conta) usando este e-mail: {data.Email}.
                    O convite aparece em "Meus convites" e vale até {DisplayTime.Format(data.ExpiresAt)} (horário de Brasília).

                    Se você não esperava este convite, basta ignorar este e-mail.
                    """)
                .Build();

            await emailQueue.EnqueueAsync(message, cancellationToken);
        }
    }
}
