using MediatR;

namespace Sienna.Application.UseCases.Workflow.CreateCampaign.CampaignCreated
{
    internal record CampaignCreatedNotification(string CampaignName, Guid TeamId) : INotification;
}
