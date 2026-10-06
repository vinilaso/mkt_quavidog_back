namespace Sienna.Domain.Abstractions.Workflow.DTOs.Teams
{
    /// <summary>Campanhas de um time.</summary>
    public class TeamCampaignsDTO
    {
        /// <summary>ID do time.</summary>
        public Guid TeamId { get; set; }
        /// <summary>Campanhas do time.</summary>
        public IEnumerable<TeamCampaignDTO> Campaigns { get; set; } = [];
    }
}
