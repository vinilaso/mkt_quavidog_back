using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions.Workflow.Repositories;

namespace Sienna.Application.UseCases.Workflow.CreateCampaign.CampaignCreated
{
    internal class CampaignCreatedNotificationHandler(IEmailQueue emailQueue, ITeamRepository teamRepository) : INotificationHandler<CampaignCreatedNotification>
    {
        public async Task Handle(CampaignCreatedNotification notification, CancellationToken cancellationToken)
        {
            var team = await teamRepository.FindByIdAsync(notification.TeamId, cancellationToken);

            if (team is null)
            {
                return;
            }

            foreach (var member in team.Members)
            {
                if (string.IsNullOrWhiteSpace(member?.Member?.Email))
                {
                    continue;
                }

                var mailMessage = new MailMessageBuilder()
                    .AddRecipient(member.Member.Email)
                    .AddSubject($"Nova campanha criada para o time {team.Name}")
                    .AddPlainBody($"A campanha '{notification.CampaignName}' foi criada no time {team.Name}. Acesse o sistema para mais detalhes.")
                    .Build();

                await emailQueue.EnqueueAsync(mailMessage, cancellationToken);
            }
        }
    }
}
