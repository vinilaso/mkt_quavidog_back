using MediatR;
using Sienna.Application.UseCases.Workflow.CreateCampaign.CampaignCreated;
using Sienna.Domain.Abstractions;
using Sienna.Domain.Abstractions.Results;
using Sienna.Domain.Abstractions.Workflow.Repositories;
using Sienna.Domain.Entities.Workflow;

namespace Sienna.Application.UseCases.Workflow.CreateCampaign
{
    public sealed class CreateCampaignHandler(IUnitOfWork uow, ICampaignRepository campaignRepository, IPublisher publisher) : IRequestHandler<CreateCampaignCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
        {
            var campaign = new Campaign(request.CampaignName, request.TeamId);

            await campaignRepository.AddAsync(campaign, cancellationToken);
            await uow.CommitChangesAsync(cancellationToken);

            await publisher.Publish(new CampaignCreatedNotification(campaign.Name, campaign.TeamId!.Value), cancellationToken);

            return campaign.Id;
        }
    }
}
