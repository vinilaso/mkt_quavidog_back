using Sienna.Application.UseCases.Workflow.CreateCampaign;

namespace Sienna.WebApi.Endpoints.Models.Workflow.Campaigns
{
    /// <summary>Dados para criar uma campanha.</summary>
    /// <param name="CampaignName">Nome da campanha.</param>
    public record CreateCampaignRequest(string CampaignName)
    {
        public CreateCampaignCommand ToCommand(Guid teamId) => new(CampaignName, teamId);
    }
}
