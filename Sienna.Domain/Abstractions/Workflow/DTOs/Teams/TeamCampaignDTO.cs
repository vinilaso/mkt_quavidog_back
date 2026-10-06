namespace Sienna.Domain.Abstractions.Workflow.DTOs.Teams
{
    /// <summary>Campanha de um time.</summary>
    public class TeamCampaignDTO
    {
        /// <summary>ID da campanha.</summary>
        public Guid CampaignId { get; set; }
        /// <summary>Nome da campanha.</summary>
        public string? Name { get; set; }
        /// <summary>Situação da campanha: Active ou Inactive.</summary>
        public string? Status { get; set; }
    }
}
