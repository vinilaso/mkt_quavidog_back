namespace Sienna.Domain.Abstractions.Workflow.DTOs.Teams
{
    public class TeamCampaignsDTO
    {
        public Guid TeamId { get; set; }
        public IEnumerable<TeamCampaignDTO> Campaigns { get; set; } = [];
    }
}
