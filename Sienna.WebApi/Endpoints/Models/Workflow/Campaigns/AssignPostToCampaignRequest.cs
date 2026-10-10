using Sienna.Application.UseCases.Workflow.AssignPostToCampaign;

namespace Sienna.WebApi.Endpoints.Models.Workflow.Campaigns
{
    /// <summary>Dados para associar uma postagem a uma campanha.</summary>
    /// <param name="PostId">ID da postagem a ser associada.</param>
    public record AssignPostToCampaignRequest(Guid PostId)
    {
        public AssignPostToCampaignCommand ToCommand(Guid teamId, Guid campaignId) => new(teamId, campaignId, PostId);
    }
}
